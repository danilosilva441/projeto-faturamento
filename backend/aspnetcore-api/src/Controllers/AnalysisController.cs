using FaturamentoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FaturamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnalysisController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // O endpoint de média que já tínhamos
        [HttpGet("daily-average/{operacaoId}")]
        public async Task<IActionResult> GetDailyAverage(int operacaoId)
        {
            var valores = await _context.Faturamentos
                                        .Where(f => f.OperacaoId == operacaoId)
                                        .Select(f => f.Valor)
                                        .ToListAsync();
            if (!valores.Any()) return NotFound($"Nenhum faturamento para a operação {operacaoId}.");

            var requestData = new { valores };
            
            // --- A MESMA CORREÇÃO SERÁ APLICADA AQUI ---
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var client = _httpClientFactory.CreateClient("AnalysisService");
                var response = await client.PostAsync("analysis/average", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<JsonElement>(responseBody));
                }
                var errorBody = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erro no serviço de análise: {errorBody}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Serviço de análise indisponível: {ex.Message}");
            }
        }

        // --- NOVO ENDPOINT DE PREVISÃO ---
        // ROTA: GET -> http://localhost:5013/api/analysis/forecast/1
        [HttpGet("forecast/{operacaoId}")]
        public async Task<IActionResult> GetForecast(int operacaoId)
        {
            // 1. Busca os dados históricos (Data e Valor) do banco
            var historico = await _context.Faturamentos
                                          .Where(f => f.OperacaoId == operacaoId)
                                          .Select(f => new { f.Data, f.Valor })
                                          .ToListAsync();

            if (!historico.Any())
            {
                return NotFound($"Não há dados históricos suficientes para a operação {operacaoId}.");
            }
            
            // 2. Prepara os dados no formato que o microserviço Node.js espera
            var requestData = new { historico };
            
            // --- A CORREÇÃO ESTÁ AQUI ---
            // Criamos opções para o serializador de JSON...
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                // ...e dizemos a ele para usar camelCase nos nomes das propriedades!
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            // Agora, usamos essas opções ao converter o nosso objeto para JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");

            // 3. Chama o microserviço Node.js
            try
            {
                var client = _httpClientFactory.CreateClient("AnalysisService");
                var response = await client.PostAsync("analysis/forecast", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<JsonElement>(responseBody));
                }
                var errorBody = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erro no serviço de previsão: {errorBody}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Serviço de previsão indisponível: {ex.Message}");
            }
        }
    }
}


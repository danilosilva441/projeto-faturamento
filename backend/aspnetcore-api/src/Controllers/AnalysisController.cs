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

        // --- NOVO ENDPOINT DE PROGRESSO DE META ---
        // ROTA: GET -> /api/analysis/progresso-meta/1
        [HttpGet("progresso-meta/{operacaoId}")]
        public async Task<IActionResult> GetProgressoMetaMensal(int operacaoId)
        {
            // 1. Busca a operação para obter a sua meta mensal
            var operacao = await _context.Operacoes.FindAsync(operacaoId);
            if (operacao == null)
            {
                return NotFound("Operação não encontrada.");
            }

            // Define o período atual (primeiro e último dia do mês corrente)
            var hoje = DateTime.UtcNow;
            var primeiroDiaDoMes = new DateTime(hoje.Year, hoje.Month, 1);
            var ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            // 2. Calcula o total faturado para a operação no mês atual
            var totalFaturadoNoMes = await _context.Faturamentos
                .Where(f => 
                    f.OperacaoId == operacaoId && 
                    f.Data >= primeiroDiaDoMes && 
                    f.Data <= ultimoDiaDoMes)
                .SumAsync(f => f.Valor);

            // 3. Calcula os resultados
            var meta = operacao.MetaMensal;
            var progressoPercentual = (meta > 0) ? (totalFaturadoNoMes / meta) * 100 : 0;
            var valorRestante = Math.Max(0, meta - totalFaturadoNoMes); // Garante que não seja negativo

            // 4. Monta o objeto de resposta
            var resultado = new {
                OperacaoNome = operacao.Nome,
                MetaMensal = meta,
                TotalFaturado = totalFaturadoNoMes,
                ProgressoPercentual = Math.Round(progressoPercentual, 2),
                ValorRestante = valorRestante,
                MesReferencia = primeiroDiaDoMes.ToString("MMMM/yyyy")
            };
            
            return Ok(resultado);
        }

        // --- Endpoints antigos ---
        [HttpGet("daily-average/{operacaoId}")]
        public async Task<IActionResult> GetDailyAverage(int operacaoId)
        {
            var valores = await _context.Faturamentos
                                        .Where(f => f.OperacaoId == operacaoId)
                                        .Select(f => f.Valor)
                                        .ToListAsync();
            if (!valores.Any()) return NotFound($"Nenhum faturamento para a operação {operacaoId}.");

            var requestData = new { valores };
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, jsonOptions), System.Text.Encoding.UTF8, "application/json");

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
        
        [HttpGet("forecast/{operacaoId}")]
        public async Task<IActionResult> GetForecast(int operacaoId)
        {
            var historico = await _context.Faturamentos
                                          .Where(f => f.OperacaoId == operacaoId)
                                          .Select(f => new { f.Data, f.Valor })
                                          .ToListAsync();
            if (!historico.Any()) return NotFound($"Não há dados históricos suficientes para a operação {operacaoId}.");
            
            var requestData = new { historico };
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, jsonOptions), System.Text.Encoding.UTF8, "application/json");

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


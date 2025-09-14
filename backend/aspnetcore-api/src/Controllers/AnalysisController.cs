using FaturamentoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json; // Importante para trabalhar com JSON

namespace FaturamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        // Injetamos o nosso DbContext (para aceder ao banco) E o HttpClientFactory (para chamar o outro serviço)
        public AnalysisController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // --- ENDPOINT DE TESTE DE INTEGRAÇÃO ---
        // ROTA: GET -> http://localhost:5013/api/analysis/daily-average/1
        [HttpGet("daily-average/{operacaoId}")]
        public async Task<IActionResult> GetDailyAverage(int operacaoId)
        {
            // 1. BUSCAR DADOS DO BANCO POSTGRES
            // Busca todos os valores de faturamento para a operação especificada.
            var valores = await _context.Faturamentos
                                        .Where(f => f.OperacaoId == operacaoId)
                                        .Select(f => f.Valor) // Seleciona apenas a coluna "valor"
                                        .ToListAsync();

            if (!valores.Any())
            {
                return NotFound($"Nenhum faturamento encontrado para a operação com ID {operacaoId}.");
            }

            // 2. PREPARAR DADOS PARA O MICROSERVIÇO NODE.JS
            // Criamos um objeto anónimo que tem a mesma "forma" que o DTO do NestJS espera
            var requestData = new { valores = valores };
            
            // Convertemos este objeto C# para uma string JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestData),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // 3. CHAMAR O MICROSERVIÇO NODE.JS
            try
            {
                // Pega o cliente HTTP que configurámos no Program.cs
                var client = _httpClientFactory.CreateClient("AnalysisService");

                // Faz a requisição POST para o endpoint do Node.js
                var response = await client.PostAsync("analysis/average", jsonContent);

                // Se a resposta do Node.js for um sucesso...
                if (response.IsSuccessStatusCode)
                {
                    // Lê o corpo da resposta (que será '{"mediaCalculada": 26.38}')
                    var responseBody = await response.Content.ReadAsStringAsync();
                    
                    // Converte a resposta JSON de volta para um objeto C# e retorna para o utilizador
                    var result = JsonSerializer.Deserialize<JsonElement>(responseBody);
                    return Ok(result);
                }
                else
                {
                    // Se o Node.js deu um erro, repassa esse erro
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Erro ao chamar o serviço de análise: {errorBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Este erro acontece se o microserviço Node.js estiver offline
                return StatusCode(503, $"Serviço de análise indisponível: {ex.Message}");
            }
        }
    }
}
/*
```

---

### **Passo 3: O Teste de Integração Final**

Agora, a hora da verdade. Vamos testar se a "ponte" entre os seus dois serviços está a funcionar.

1.  **Inicie os DOIS servidores:**
    * Num terminal, na pasta `.../backend/aspnetcore-api/src`, rode `dotnet run`.
    * Num **outro** terminal, na pasta `.../backend/node-microservice`, rode `yarn start:dev`.
    * **É crucial que ambos estejam a rodar ao mesmo tempo.**

2.  **Faça o Teste:**
    * Abra o seu navegador.
    * Aceda ao **novo endpoint da sua API .NET**:
        **`http://localhost:5013/api/analysis/daily-average/1`**
        *(Isto irá pedir a média para a Operação "Motorista App - Danilo")*

**Resultado Esperado:**
Você não verá "Hello World!". Em vez disso, o seu navegador deverá mostrar um resultado JSON que foi **calculado pelo seu serviço Node.js**, algo parecido com isto:

```json
{
  "mediaCalculada": 2005.78 
}

*/
using FaturamentoApi.Data; // Importa nosso DbContext
using FaturamentoApi.Models; // Importa nossos Models
using Microsoft.AspNetCore.Mvc; // Importa as classes base de API do .NET
using Microsoft.EntityFrameworkCore; // Importa os métodos do EF Core (como ToListAsync)

namespace FaturamentoApi.Controllers
{
    [ApiController] // Marca esta classe como um Controller de API (habilita recursos automáticos)
    [Route("api/[controller]")] // Define a rota base da URL. [controller] = "operacoes" (o nome da classe sem "Controller")
    public class OperacoesController : ControllerBase // Classe base para controllers de API
    {
        // 1. Variável privada para guardar a conexão com o banco
        private readonly AppDbContext _context;

        // 2. Construtor (Injeção de Dependência)
        // Quando uma requisição chegar aqui, o ASP.NET vai "injetar" (entregar) automaticamente
        // o serviço do AppDbContext (que registramos no Program.cs) para nós.
        public OperacoesController(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODO 1: BUSCAR TODAS AS OPERAÇÕES ---
        // ROTA: GET -> http://localhost:5013/api/operacoes
        [HttpGet]
        public async Task<IActionResult> GetTodasOperacoes()
        {
            // Explicação:
            // async Task<IActionResult>: Dizemos que este método é assíncrono (não trava o servidor) 
            //                          e que vai retornar uma Resposta HTTP (IActionResult).
            
            // await _context.Operacoes.ToListAsync():
            // 1. await: "Espere" esta operação terminar sem travar.
            // 2. _context.Operacoes: "Pegue a tabela de Operações".
            // 3. .ToListAsync(): "Converta tudo para uma Lista C#". (TRADUÇÃO SQL: SELECT * FROM faturamento.operacoes)
            var operacoes = await _context.Operacoes.ToListAsync();

            // return Ok(operacoes): Retorna um código HTTP 200 (OK) e envia a lista de operações como JSON.
            return Ok(operacoes);
        }

        // --- MÉTODO 2: BUSCAR UMA OPERAÇÃO POR ID ---
        // ROTA: GET -> http://localhost:5013/api/operacoes/1  (ou /2, /3, etc.)
        [HttpGet("{id}")] // A rota agora espera um parâmetro. Ex: /api/operacoes/5
        public async Task<IActionResult> GetOperacaoPorId(int id) // O 'int id' aqui é automaticamente preenchido com o valor do "{id}" da URL
        {
            // Explicação:
            // await _context.Operacoes.FindAsync(id):
            // Este é o método MAIS RÁPIDO para buscar algo pela sua Chave Primária (PK).
            // (TRADUÇÃO SQL: SELECT * FROM faturamento.operacoes WHERE id = @id LIMIT 1)
            var operacao = await _context.Operacoes.FindAsync(id);

            // Se o FindAsync não encontrar nada (ex: buscar ID 99), ele retorna null.
            // Precisamos tratar isso.
            if (operacao == null)
            {
                // return NotFound(): Retorna um código HTTP 404 (Not Found / Não Encontrado).
                return NotFound("Operação não encontrada.");
            }

            // Se encontrou, retorna HTTP 200 (OK) e o objeto JSON da operação encontrada.
            return Ok(operacao);
        }
    }
}
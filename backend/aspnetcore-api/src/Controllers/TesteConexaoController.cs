using FaturamentoApi.Data; // Importa o Contexto
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteConexaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        // O .NET vai "injetar" o AppDbContext (que registramos no Program.cs) aqui
        public TesteConexaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("usuarios")]
        public IActionResult GetUsuariosDoBanco()
        {
            try
            {
                // Tenta ler a tabela de usuarios
                var usuarios = _context.Usuarios.ToList();

                if (!usuarios.Any())
                {
                    return NotFound("Conectado ao banco, mas tabela 'faturamento.usuarios' está vazia.");
                }

                // Se der certo, retorna os usuários que o SEED criou!
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Se falhar (ex: senha errada, tabela não encontrada), retorna o erro.
                return BadRequest($"Falha ao conectar ou consultar: {ex.Message}");
            }
        }

        [HttpGet("dados-completos")]
        public IActionResult GetDadosCompletos()
        {
            try
            {
                // ESTA É A CONSULTA:
                // 1. Busque todas as Operações (_context.Operacoes)
                // 2. .Include(op => op.Faturamentos) -> TRADUÇÃO: "Faça um JOIN com a tabela Faturamentos"
                // 3. .Include(op => op.Metas) -> TRADUÇÃO: "Faça também um JOIN com a tabela Metas"
                var operacoesComDados = _context.Operacoes
                                                .Include(op => op.Faturamentos)
                                                .Include(op => op.MetaMensal)
                                                .ToList(); // Executa a consulta no banco

                if (!operacoesComDados.Any())
                {
                    return NotFound("Operações encontradas, mas nenhum dado relacionado (verifique o seed).");
                }

                // Para não poluir sua tela com 362 faturamentos, vamos retornar um resumo:
                var resumo = operacoesComDados.Select(op => new
                {
                    operacaoNome = op.Nome,
                    totalFaturamentosRegistrados = op.Faturamentos.Count(), // Conta quantos faturamentos achou
                     // Conta quantas metas achou
                    primeiroFaturamentoExemplo = op.Faturamentos.OrderBy(f => f.Data).FirstOrDefault()?.Valor
                });

                return Ok(resumo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Falha ao consultar dados completos: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
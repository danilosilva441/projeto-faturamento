using FaturamentoApi.Data;
using FaturamentoApi.Dtos;
using FaturamentoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaturamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FaturamentosController(AppDbContext context)
        {
            _context = context;
        }

        // --- NOVO MÉTODO GET ---
        // ROTA: GET -> http://localhost:5013/api/faturamentos
        [HttpGet]
        public async Task<IActionResult> GetTodosFaturamentos()
        {
            var faturamentos = await _context.Faturamentos
                                             .Include(f => f.Operacao)
                                             .OrderByDescending(f => f.Data)
                                             .Take(50)
                                             .ToListAsync();

            return Ok(faturamentos);
        }

        // --- MÉTODO POST (MELHORADO) ---
        [HttpPost]
        public async Task<IActionResult> CriarLancamento([FromBody] CriarFaturamentoDto dto)
        {
            if (dto.DataRef.Date > DateTime.UtcNow.Date)
            {
                return BadRequest("Não é permitido lançar faturamentos para datas futuras.");
            }
            
            var operacaoExiste = await _context.Operacoes.AnyAsync(op => op.Id == dto.OperacaoId && op.Ativo);
            if (!operacaoExiste)
            {
                return NotFound("A operação informada não existe ou está inativa.");
            }

            var novoFaturamento = new FaturamentoDiario
            {
                OperacaoId = dto.OperacaoId,
                Data = dto.DataRef.Date,
                Valor = dto.Valor,
                CriadoEm = DateTime.UtcNow
            };

            try
            {
                _context.Faturamentos.Add(novoFaturamento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            when (ex.InnerException is Npgsql.PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                return Conflict("Já existe um lançamento cadastrado para esta operação nesta data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
            
            var faturamentoCriado = await _context.Faturamentos
                                                  .Include(f => f.Operacao)
                                                  .FirstOrDefaultAsync(f => f.Id == novoFaturamento.Id);

            return Created("", faturamentoCriado);
        }
    }
}
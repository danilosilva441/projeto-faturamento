using FaturamentoApi.Data;         // Importa o Contexto do Banco
using FaturamentoApi.Dtos;         // Importa nosso novo DTO
using FaturamentoApi.Models;       // Importa os Models do Banco (FaturamentoDiario)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Para acessar o ".AnyAsync()" e tratar exceções

namespace FaturamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Rota: /api/faturamentos
    public class FaturamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeção de Dependência do nosso Contexto do banco (igual ao OperacoesController)
        public FaturamentosController(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODO 3: CRIAR NOVO LANÇAMENTO (O "C" do CRUD) ---
        // ROTA: POST -> http://localhost:5013/api/faturamentos
        [HttpPost]
        public async Task<IActionResult> CriarLancamento([FromBody] CriarFaturamentoDto dto)
        {
            // [FromBody]: Diz à API para ler o JSON do corpo da requisição e tentar encaixá-lo no "molde" (CriarFaturamentoDto).
            // Se as validações (Required, Range) do DTO falharem, a API retorna Erro 400 automaticamente aqui.

            // --- Validações de Regra de Negócio (que o DTO não pega) ---

            // REGRA 1 (do seu briefing): Não permitir datas futuras.
            // Comparamos apenas a parte da Data (ignorando horas/minutos)
            if (dto.DataRef.Date > DateTime.UtcNow.Date)
            {
                // return BadRequest: Retorna HTTP 400 (Requisição Ruim) com uma mensagem clara.
                return BadRequest("Não é permitido lançar faturamentos para datas futuras.");
            }

            // REGRA 2 (Integridade): A Operação (OperacaoId) que o usuário enviou existe no banco?
            var operacaoExiste = await _context.Operacoes.AnyAsync(op => op.Id == dto.OperacaoId && op.Ativo);
            if (!operacaoExiste)
            {
                // return NotFound: Retorna HTTP 404 (Não Encontrado).
                return NotFound("A operação informada não existe ou está inativa.");
            }

            // --- Se passou em todas as validações, criamos o Model ---

            // Convertemos o DTO (dados de entrada) para o Model (o que vai ser salvo no banco)
            var novoFaturamento = new FaturamentoDiario
            {
                OperacaoId = dto.OperacaoId,
                Data = dto.DataRef.Date, // Garantimos que salvamos apenas a Data (sem hora)
                Valor = dto.Valor,
                CriadoEm = DateTime.UtcNow // O servidor SEMPRE define a data de criação
            };

            // --- Tratamento de Erro (REGRA 3: Duplicidade) ---
            // Nosso banco tem uma restrição UNIQUE (OperacaoId, Data). Se tentarmos salvar
            // um lançamento duplicado, o banco dará um erro. Precisamos "capturar" esse erro.
            try
            {
                _context.Faturamentos.Add(novoFaturamento); // Adiciona o objeto na "fila" do EF
                await _context.SaveChangesAsync(); // Tenta salvar no banco
            }
            catch (DbUpdateException ex)
            // PostgresException é a exceção específica do Npgsql
            when (ex.InnerException is Npgsql.PostgresException postgresEx && postgresEx.SqlState == "23505") // 23505 = unique_violation
            {
                // return Conflict: Retorna HTTP 409 (Conflito). É a resposta correta para "Recurso já existe".
                return Conflict("Já existe um lançamento cadastrado para esta operação nesta data.");
            }
            catch (Exception ex)
            {
                // Qualquer outro erro inesperado
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }

            // SUCESSO!
            // Retorna HTTP 201 (Created), que é a resposta padrão para um POST bem-sucedido.
            // Também enviamos de volta o objeto que foi criado (agora com o novo 'Id' e 'CriadoEm').
            return Created("", novoFaturamento);
        }

        // ... (final do método CriarLancamento [HttpPost])

        // --- MÉTODO 4: CANCELAR UM LANÇAMENTO (O "D" do CRUD - Soft Delete) ---
        // ROTA: DELETE -> http://localhost:5013/api/faturamentos/15
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarLancamento(int id)
        {
            // O QUE É IgnoreQueryFilters()?
            // Como definimos um Filtro Global (passo 3) que SÓ busca "ativo = true",
            // um FindAsync normal não acharia um registro se ele já estivesse cancelado (ativo = false).
            // Para o método de cancelamento, precisamos IGNORAR esse filtro global 
            // para conseguirmos buscar o registro e verificar seu estado.
            var lancamento = await _context.Faturamentos
                                           .IgnoreQueryFilters() // Ignora o "WHERE ativo = true"
                                           .FirstOrDefaultAsync(f => f.Id == id); // Busca o ID

            // Validação 1: O ID existe?
            if (lancamento == null)
            {
                return NotFound("Lançamento de faturamento não encontrado.");
            }

            // Validação 2: Já não está cancelado?
            if (!lancamento.Ativo) // Se Ativo for FALSE
            {
                return BadRequest("Este lançamento já se encontra cancelado.");
            }

            // O "Delete" Lógico (Soft Delete):
            lancamento.Ativo = false;
            // (Poderíamos adicionar também um campo "CanceladoPorUsuarioId" ou "DataCancelamento" no futuro)

            await _context.SaveChangesAsync(); // Salva o UPDATE no banco

            // return NoContent(): Esta é a resposta HTTP padrão (204) para um DELETE
            // bem-sucedido, que significa "Eu fiz o que você pediu e não tenho nada para te devolver".
            return NoContent();
        }

                // --- MÉTODO 5: ATUALIZAR UM LANÇAMENTO (O "U" do CRUD) ---
        // ROTA: PUT -> http://localhost:5013/api/faturamentos/15
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarLancamento(int id, [FromBody] AtualizarFaturamentoDto dto)
        {
            // O [HttpPut("{id}")] espera o ID na URL e o JSON de atualização no corpo (body).
            
            // 1. BUSCAR O REGISTRO NO BANCO
            // Assim como no DELETE, precisamos IGNORAR os filtros globais (HasQueryFilter),
            // pois um Admin pode precisar corrigir um lançamento que já está cancelado (ativo = false).
            var lancamentoParaAtualizar = await _context.Faturamentos
                                                        .IgnoreQueryFilters() 
                                                        .FirstOrDefaultAsync(f => f.Id == id);

            // Validação 1: O lançamento existe?
            if (lancamentoParaAtualizar == null)
            {
                return NotFound("Lançamento de faturamento não encontrado.");
            }

            // --- VALIDAÇÕES DE REGRA DE NEGÓCIO (ANTES DE SALVAR) ---

            // REGRA 1 (do seu briefing): Não permitir datas futuras, mesmo na atualização.
            if (dto.DataRef.Date > DateTime.UtcNow.Date)
            {
                return BadRequest("Não é permitido atualizar lançamentos para datas futuras.");
            }

            // REGRA 2 (Duplicidade): O usuário está tentando mudar a data para um dia
            // que JÁ TEM um lançamento (para esta mesma operação)?
            // Ex: Lançamento ID 5 (dia 01/Jan) e ID 6 (dia 02/Jan). 
            // Se tentarmos mudar o ID 5 para 02/Jan, o banco (UNIQUE constraint) vai falhar.
            
            bool dataJaExiste = await _context.Faturamentos.AnyAsync(f => 
                f.OperacaoId == lancamentoParaAtualizar.OperacaoId && // Para a mesma operação
                f.Data == dto.DataRef.Date &&                           // Na data que estamos tentando salvar
                f.Id != id);                                            // E que NÃO SEJA ele mesmo
            
            if (dataJaExiste)
            {
                // return Conflict (409): Já existe um recurso nesse estado.
                return Conflict("Já existe um lançamento cadastrado para esta operação nesta data. A atualização foi bloqueada para evitar duplicidade.");
            }

            // --- ATUALIZAÇÃO ---
            // Se passou em tudo, atualizamos os dados do objeto que buscamos do banco:
            lancamentoParaAtualizar.Data = dto.DataRef.Date;
            lancamentoParaAtualizar.Valor = dto.Valor;
            // (Poderíamos adicionar um campo 'AtualizadoEm = DateTime.UtcNow' aqui se a tabela tivesse)

            // Tratamento de Erro (igual ao POST, para garantir a captura da violação UNIQUE caso ocorra uma 'race condition')
            try
            {
                await _context.SaveChangesAsync(); // Salva as mudanças (Executa o UPDATE no banco)
            }
            catch (DbUpdateException ex) 
            when (ex.InnerException is Npgsql.PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                return Conflict("Erro de concorrência. Já existe um lançamento para esta operação nesta data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }

            // SUCESSO!
            // return Ok(objeto): Retorna HTTP 200 (OK) e envia o objeto atualizado de volta.
            return Ok(lancamentoParaAtualizar);
        }
    }
}
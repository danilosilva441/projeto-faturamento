Backend API - Faturamento (.NET 8)
Esta pasta contém a API REST principal do projeto, desenvolvida com ASP.NET Core 8. Ela é responsável por toda a lógica de negócio, interações com o banco de dados e por fornecer os dados para o frontend.

Arquitetura e Padrões
API RESTful: Endpoints bem definidos para cada recurso (/api/operacoes, /api/faturamentos).

Entity Framework Core: Utilizado como ORM (Object-Relational Mapper) para a comunicação com o banco de dados PostgreSQL.

Padrão de Repositório (implícito via DbContext): Centraliza o acesso aos dados.

Injeção de Dependência: Utilizada para gerir o ciclo de vida dos serviços, como o AppDbContext.

DTOs (Data Transfer Objects): Usados para validar e modelar os dados que entram e saem da API, garantindo segurança e um "contrato" claro com o frontend.

Soft Delete: Nenhum registo de faturamento é realmente apagado do banco. Em vez disso, são marcados como inativos (ativo = false) para manter a integridade do histórico.

Endpoints Principais Implementados
A API atualmente expõe os seguintes endpoints:

GET /api/operacoes: Retorna uma lista de todas as operações cadastradas.

GET /api/operacoes/{id}: Retorna uma operação específica pelo seu ID.

GET /api/faturamentos: Retorna os últimos 50 faturamentos lançados, ordenados por data.

POST /api/faturamentos: Cadastra um novo lançamento de faturamento diário.

PUT /api/faturamentos/{id}: Atualiza um lançamento existente.

DELETE /api/faturamentos/{id}: Cancela (Soft Delete) um lançamento existente.

Como Executar
Certifique-se de que o container Docker do PostgreSQL (definido na pasta infra) está a rodar.

Navegue até à pasta backend/aspnetcore-api/src.

Execute o comando:

dotnet run

A API estará disponível em http://localhost:5013. Pode aceder à documentação interativa do Swagger em http://localhost:5013/swagger.
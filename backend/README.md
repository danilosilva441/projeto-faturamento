Backend - API de Faturamento em ASP.NET Core
Esta pasta contém o projeto da API RESTful, que serve como o núcleo do sistema de faturamento. Ela é responsável por gerenciar toda a lógica de negócio, persistência de dados e segurança da aplicação.

Tecnologias e Padrões Utilizados
Framework: ASP.NET Core 8

Linguagem: C#

Banco de Dados: PostgreSQL (gerenciado via Docker)

ORM (Object-Relational Mapper): Entity Framework Core 8

Arquitetura: API RESTful com padrão de Controllers

Padrões de Projeto:

DTOs (Data Transfer Objects): Para garantir que os "contratos" entre a API e o frontend sejam seguros e explícitos.

Injeção de Dependência: Utilizada extensivamente para gerenciar serviços como o AppDbContext.

Soft Delete: Nenhum registro financeiro é realmente apagado do banco. Em vez disso, eles são marcados como inativos (ativo = false), preservando a integridade e o histórico dos dados.

Filtros Globais (EF Core): O DbContext é configurado para, por padrão, ignorar registros inativos em todas as consultas, simplificando a lógica nos controllers.

Banco de Dados
O esquema do banco de dados é projetado para ser simples e escalável, com as seguintes tabelas principais:

faturamento.operacoes: Armazena as operações (ex: "Loja Centro", "Contrato X").

faturamento.faturamentos: Guarda os lançamentos de faturamento diário, com uma restrição UNIQUE para impedir duplicidade (mesma operação, mesmo dia).

faturamento.metas: Armazena as metas mensais por operação.

faturamento.usuarios: Tabela para futuros usuários do sistema.

O script de criação do schema e o seed (popular com dados de teste) estão localizados em ../infra/db/init/.

Endpoints Principais da API
A API expõe os seguintes recursos:

GET /api/operacoes: Retorna uma lista de todas as operações cadastradas.

GET /api/operacoes/{id}: Retorna uma operação específica pelo seu ID.

POST /api/faturamentos: Cria um novo lançamento de faturamento diário. Inclui validações para impedir datas futuras, valores zerados e duplicidade.

PUT /api/faturamentos/{id}: Atualiza um lançamento existente (data ou valor).

DELETE /api/faturamentos/{id}: Realiza um "Soft Delete", marcando um lançamento como inativo.

Como Executar Apenas o Backend
Garanta que o container do PostgreSQL (definido em ../infra/docker-compose.yml) esteja rodando.

Navegue até a pasta aspnetcore-api/src.

Execute o comando:

dotnet run

A documentação interativa da API (Swagger) estará disponível em http://localhost:[porta]/swagger.
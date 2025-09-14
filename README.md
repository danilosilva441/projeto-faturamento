Projeto de Faturamento e Previsões (Full-Stack)
Este repositório contém uma aplicação web full-stack completa para registo e análise de faturamentos diários. A aplicação permite aos utilizadores cadastrar lançamentos, visualizar dados históricos e, futuramente, obter previsões com base no histórico armazenado.

Este projeto foi construído como uma demonstração de competências em arquitetura de software moderna, utilizando .NET para o backend, Vue.js para o frontend e Docker para a infraestrutura de banco de dados.

Status do Projeto
Fase 1 - Aplicação Principal (CRUD): CONCLUÍDA ✅

Backend API em .NET 8 com CRUD completo e robusto para operações e faturamentos.

Frontend em Vue 3 com uma interface reativa para visualização e cadastro de dados.

Banco de dados PostgreSQL a rodar num container Docker, com seed automático de dados.

Comunicação entre frontend e backend a funcionar perfeitamente.

Tecnologias Utilizadas
Backend: ASP.NET Core 8, Entity Framework Core, C#

Frontend: Vue 3, TypeScript, Vite, TailwindCSS, Axios

Banco de Dados: PostgreSQL 15

Infraestrutura: Docker e Docker Compose

Como Executar o Projeto Localmente
Para executar esta aplicação, você precisará ter o .NET SDK 8+, Node.js 18+ e o Docker Desktop instalados na sua máquina.

Siga estes passos na ordem correta:

Clone o Repositório:

git clone [https://github.com/danilosilva441/projeto-faturamento.git](https://github.com/danilosilva441/projeto-faturamento.git)
cd projeto-faturamento

Inicie o Banco de Dados (Docker):
Navegue até à pasta infra e suba os containers do PostgreSQL e do pgAdmin.

cd infra
docker compose up -d

Isto irá criar o banco de dados e populá-lo com dados de teste automaticamente.

Inicie o Backend (API .NET):
Abra um novo terminal, navegue até à pasta da API e inicie o servidor.

cd backend/aspnetcore-api/src
dotnet run

O backend estará a rodar em http://localhost:5013.

Inicie o Frontend (Vue.js):
Abra um terceiro terminal, navegue até à pasta do frontend, instale as dependências (apenas na primeira vez) e inicie o servidor de desenvolvimento.

cd frontend/vue-app
yarn install  # Apenas na primeira vez
yarn dev

Aceda à Aplicação:
Abra o seu navegador e aceda ao endereço fornecido pelo Vite, que geralmente é:
http://localhost:5173

Agora você deverá ver a aplicação completa e funcional no seu navegador!
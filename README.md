Projeto de Faturamento e Previsões (Full-Stack)
Este repositório contém o código-fonte de uma aplicação full-stack completa, projetada para registrar o faturamento diário de múltiplas operações, consolidar dados, comparar com metas e, futuramente, gerar previsões com base no histórico.

O projeto serve como um case prático de arquitetura de software moderna, utilizando uma API robusta no backend, uma interface reativa no frontend e infraestrutura containerizada com Docker.

Visão Geral da Arquitetura
A aplicação é dividida em três grandes pilares que se comunicam entre si:

Backend (API Principal): Desenvolvido em ASP.NET Core, é o cérebro do sistema. Responsável por todas as regras de negócio, validações, segurança e comunicação com o banco de dados.

Frontend (Interface do Usuário): Uma Single-Page Application (SPA) desenvolvida com Vue.js 3 (usando Vite e TypeScript). É a interface com a qual o usuário interage, consumindo os dados fornecidos pela API. A estilização é feita com TailwindCSS.

Infraestrutura (Banco de Dados & Orquestração): O banco de dados relacional PostgreSQL é executado dentro de um container Docker, garantindo um ambiente de desenvolvimento e produção consistente e isolado. O docker-compose orquestra os serviços.

Estrutura de Pastas
O projeto está organizado da seguinte forma para facilitar a manutenção e o desenvolvimento:

📁 /backend/: Contém todo o código-fonte da API em ASP.NET Core.

📁 /aspnetcore-api/: O projeto principal da API.

📁 /frontend/: Contém todo o código-fonte da aplicação Vue.js.

📁 /vue-app/: O projeto principal do frontend.

📁 /infra/: Contém os arquivos de configuração do Docker e os scripts de inicialização do banco de dados.

📁 /docs/ (sugestão): Pasta para documentação adicional, diagramas de arquitetura, etc.

Como Executar o Projeto Completo
Para subir todo o ambiente (Backend, Frontend e Banco de Dados), você precisará ter o Docker Desktop e o SDK do .NET instalados.

Subir a Infraestrutura (Banco de Dados):

cd infra
docker compose up -d

Executar o Backend (API):
Abra um novo terminal.

cd backend/aspnetcore-api/src
dotnet run

A API estará rodando, geralmente em http://localhost:5013.

Executar o Frontend:
Abra um terceiro terminal.

cd frontend/vue-app
yarn dev

A aplicação estará acessível, geralmente em http://localhost:5173.

Para mais detalhes sobre cada parte do projeto, consulte os arquivos README.md específicos dentro das pastas /backend e /frontend.
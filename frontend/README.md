Frontend - Painel de Faturamento em Vue.js
Esta pasta contém o projeto da Single-Page Application (SPA) que serve como a interface do usuário para o sistema de faturamento. A aplicação é moderna, reativa e construída com as melhores práticas do ecossistema Vue.js.

Tecnologias e Ferramentas
Framework: Vue.js 3 (com a Composition API)

Build Tool: Vite (para desenvolvimento e build ultrarrápidos)

Linguagem: TypeScript (para adicionar tipagem estática e segurança ao código)

Estilização: TailwindCSS (um framework CSS "utility-first" para criar designs customizados rapidamente)

Requisições HTTP: Axios (para comunicação com a API backend)

Gerenciador de Pacotes: Yarn

Funcionalidades
Atualmente, a aplicação consiste em uma tela principal que:

Exibe o título "Painel de Faturamento".

Ao ser carregada, faz uma chamada GET para o endpoint /api/operacoes do backend.

Mostra uma mensagem de "Carregando..." enquanto espera a resposta da API.

Exibe uma mensagem de erro clara se a comunicação com o backend falhar (ex: API offline ou erro de CORS).

Renderiza os dados recebidos em uma tabela estilizada, mostrando o ID, nome e descrição de cada operação.

Como Executar Apenas o Frontend
Pré-requisito: A API backend (../backend/aspnetcore-api) deve estar rodando, pois o frontend depende dela para buscar os dados.

Navegue até a pasta vue-app.

Se for a primeira vez, instale as dependências:

yarn install

Inicie o servidor de desenvolvimento:

yarn dev

A aplicação estará disponível em http://localhost:5173 (ou a porta indicada no terminal). O Hot Reload está ativado, então qualquer alteração no código será refletida instantaneamente no navegador.
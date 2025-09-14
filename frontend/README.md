Frontend - Painel de Faturamento (Vue 3)
Esta pasta contém a interface do utilizador (UI) do projeto, desenvolvida com Vue 3, Vite e TypeScript. É uma Single-Page Application (SPA) que consome os dados fornecidos pela API .NET.

Arquitetura e Padrões
Componentização: A aplicação é dividida em componentes reutilizáveis e com responsabilidades únicas (ex: FormularioCadastro.vue, FaturamentosLista.vue), orquestrados pelo componente principal App.vue.

Reatividade: Utiliza a Composition API do Vue 3 (ref, onMounted) para criar uma interface que se atualiza automaticamente em resposta a mudanças nos dados.

Separação de Responsabilidades:

services/apiService.ts: Centraliza toda a lógica de comunicação com o backend (usando axios).

types/index.ts: Define as "interfaces" TypeScript, criando um "dicionário" para a forma dos dados (Operacao, Faturamento), o que garante segurança de tipos e ajuda a prevenir bugs.

Estilização Utilitária: Usa TailwindCSS para criar uma interface moderna e responsiva de forma rápida e consistente.

Funcionalidades Implementadas
Visualização da lista de operações cadastradas.

Visualização dos últimos faturamentos lançados.

Formulário para cadastro de novos faturamentos com validação de dados.

Atualização dinâmica da lista de faturamentos após um novo cadastro, proporcionando feedback em tempo real ao utilizador.

Como Executar
Certifique-se de que a API .NET (definida na pasta backend) está a rodar.

Navegue até à pasta frontend/vue-app.

Instale as dependências (apenas na primeira vez):

yarn install

Inicie o servidor de desenvolvimento do Vite:

yarn dev

Aceda à aplicação no endereço fornecido, que geralmente é http://localhost:5173.
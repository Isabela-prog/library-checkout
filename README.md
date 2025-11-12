Library System
Este projeto é um sistema simples de gerenciamento de empréstimos de livros, desenvolvido como parte de estudos em C# e manipulação de dados com arquivos JSON.

Objetivo
Criar um sistema funcional que permita:
- Registrar empréstimos de livros (Checkout)
- Atualizar a data de devolução
- Controlar disponibilidade dos livros
- Persistir dados de usuários, livros e empréstimos em arquivos JSON
- Autenticar usuários com senha segura (PBKDF2)
- Permitir que usuários gerenciem sua própria lista de livros
- Excluir conta e dados pessoais via menu interativo

Tecnologias utilizadas
- C# (.NET) — linguagem principal
- JSON — para persistência de dados
- Programação orientada a objetos — com uso de herança e composição
- Repositórios e serviços — para separação de responsabilidades
- PBKDF2 — para hash seguro de senhas
- Geocodificação — para localização automática de usuários

Estrutura do projeto
Library/
├── Models/
│   ├── Book.cs
│   ├── User.cs
│   └── Checkout.cs
├── Services/
│   ├── AuthService.cs
│   └── CheckoutServices.cs
├── Repositories/
│   ├── BookRepository.cs
│   ├── UserRepository.cs
│   └── CheckoutRepository.cs
├── Security/
│   ├── IPasswordHasher.cs
│   └── Pbkdf2PasswordHasher.cs
├── Utils/
│   ├── Validator.cs
│   └── GeoService.cs
├── Data/
│   ├── JsonContext.cs
│   ├── books.json
│   ├── users.json
│   └── checkouts.json
└── Program.cs



Como funciona
- O serviço CheckoutServices gerencia a lógica de empréstimos.
- O AuthService permite registrar, autenticar e excluir usuários.
- Após login, o usuário pode adicionar, remover e listar seus livros pessoais.
- Os repositórios manipulam os arquivos JSON.
- O JsonContext carrega e salva os dados.
- O sistema usa DateTime.Now para registrar datas de empréstimo e devolução.
- As senhas são protegidas com PBKDF2.
- A geolocalização é feita com base no endereço do usuário.

Observações
Este projeto foi criado com fins educacionais, para praticar:
- Boas práticas de arquitetura
- Manipulação de dados persistentes
- Validação de regras de negócio
- Uso de construtores, herança e composição
- Implementação de autenticação segura
- Interação com o usuário via console

Próximos passos (ideias para evolução)
- Interface gráfica ou web (WinForms, WPF ou ASP.NET)
- Validação de atraso e cálculo de multas
- Relatórios de uso
- Cadastro de categorias e autores
- Histórico de leitura por usuário
- Exportação de dados para CSV ou PDF

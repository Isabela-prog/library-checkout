Library Checkout System
Este projeto é um sistema simples de gerenciamento de empréstimos de livros, desenvolvido como parte de estudos em C# e manipulação de dados com arquivos JSON.

Objetivo
Criar um sistema funcional que permita:
- Registrar empréstimos de livros (Checkout)
- Atualizar a data de devolução
- Controlar disponibilidade dos livros
- Persistir dados de usuários, livros e empréstimos em arquivos JSON

Tecnologias utilizadas
- C# (.NET) — linguagem principal
- JSON — para persistência de dados
- Programação orientada a objetos — com uso de herança e composição
- Repositórios e serviços — para separação de responsabilidades

Estrutura do projeto
Library/
├── Models/
│   ├── Book.cs
│   ├── User.cs
│   └── Checkout.cs
├── Services/
│   └── CheckoutServices.cs
├── Repositories/
│   ├── BookRepository.cs
│   ├── UserRepository.cs
│   └── CheckoutRepository.cs
├── Data/
│   └── JsonContext.cs
└── Data/
    ├── books.json
    ├── users.json
    └── checkouts.json

Como funciona
- O serviço CheckoutServices recebe os repositórios e gerencia a lógica de empréstimos.
- Os repositórios (BookRepository, UserRepository, CheckoutRepository) manipulam os arquivos JSON.
- O JsonContext é responsável por carregar e salvar os dados.
- O sistema usa DateTime.Now para registrar datas de empréstimo e devolução.

Observações
Este projeto foi criado com fins educacionais, para praticar:
- Boas práticas de arquitetura
- Manipulação de dados persistentes
- Validação de regras de negócio
- Uso de construtores, herança e composição

Próximos passos (ideias para evolução)
- Interface gráfica ou web (WinForms, WPF ou ASP.NET)
- Validação de atraso e cálculo de multas
- Autenticação de usuários
- Relatórios de uso

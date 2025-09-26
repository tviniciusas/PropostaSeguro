# Sistema de Propostas de Seguro

Sistema desenvolvido com arquitetura hexagonal (Ports & Adapters) utilizando .NET 8, PostgreSQL, Dapper e FluentMigrator, dividido em dois microserviços:

## Arquitetura

### PropostaService
Responsável por:
- ✅ Criar proposta de seguro
- ✅ Listar propostas
- ✅ Alterar status da proposta (Em Análise, Aprovada, Rejeitada)
- ✅ Expor API REST

### ContratacaoService
Responsável por:
- ✅ Contratar uma proposta (somente se Aprovada)
- ✅ Armazenar informações da contratação
- ✅ Comunicar-se com o PropostaService via HTTP
- ✅ Expor API REST

## Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **PostgreSQL** - Banco de dados
- **Dapper** - ORM leve para acesso a dados
- **FluentMigrator** - Migrations de banco de dados
- **Docker & Docker Compose** - Containerização
- **xUnit** - Framework de testes
- **Moq** - Mock para testes unitários
- **Swagger** - Documentação da API

## Como Executar

### Pré-requisitos
- Docker e Docker Compose instalados

### Executando com Docker Compose

1. Clone o repositório
2. Na raiz do projeto, execute:

```bash
docker-compose up --build
```

Isso irá:
- Criar dois bancos PostgreSQL (propostas_db e contratacoes_db)
- Executar as migrations automaticamente
- Subir os dois microserviços

### URLs dos Serviços

- **PropostaService**: http://localhost:5001
  - Swagger: http://localhost:5001/swagger
- **ContratacaoService**: http://localhost:5002
  - Swagger: http://localhost:5002/swagger

### Bancos de Dados

- **PropostaService**: localhost:5432 (propostas_db)
- **ContratacaoService**: localhost:5433 (contratacoes_db)

## Estrutura do Projeto

```
src/
├── PropostaService/
│   ├── Domain/              # Entidades, Value Objects, Enums
│   ├── Application/         # Use Cases, DTOs, Ports
│   ├── Infrastructure/      # Repositórios, Migrations
│   └── Presentation/        # Controllers, API
├── ContratacaoService/
│   ├── Domain/              # Entidades, Value Objects, Enums
│   ├── Application/         # Use Cases, DTOs, Ports
│   ├── Infrastructure/      # Repositórios, HTTP Client
│   └── Presentation/        # Controllers, API
tests/
├── PropostaService.Tests/   # Testes unitários
└── ContratacaoService.Tests/# Testes unitários
```

## Exemplos de Uso

### 1. Criar uma Proposta

```bash
POST http://localhost:5001/api/propostas
Content-Type: application/json

{
  "nomeCliente": "João Silva",
  "cpfCliente": "12345678901",
  "emailCliente": "joao@exemplo.com",
  "tipoSeguro": 1,
  "valorCobertura": 100000.00,
  "valorPremio": 5000.00,
  "observacoes": "Proposta para seguro de vida"
}
```


### 2. Listar Propostas

```bash
GET http://localhost:5001/api/propostas
```

### 3. Aprovar uma Proposta

```bash
PUT http://localhost:5001/api/propostas/{id}/status
Content-Type: application/json

{
  "novoStatus": 2,
  "observacoes": "Documentação completa"
}
```

### 4. Contratar uma Proposta (Aprovada)

```bash
POST http://localhost:5002/api/contratacoes
Content-Type: application/json

{
  "propostaId": "guid-da-proposta-aprovada",
  "dataVigenciaInicio": "2024-01-01T00:00:00",
  "dataVigenciaFim": "2024-12-31T23:59:59"
}
```

## Executar Testes

Para executar os testes unitários:

```bash
# Executar todos os testes
dotnet test

# Executar testes com coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Princípios Aplicados

### Clean Architecture / Arquitetura Hexagonal
- **Domain**: Regras de negócio puras, sem dependências externas
- **Application**: Casos de uso e interfaces (ports)
- **Infrastructure**: Implementações concretas (adapters)
- **Presentation**: Interface com o mundo externo (API REST)

### Domain-Driven Design (DDD)
- **Entities**: Proposta, Contratacao
- **Value Objects**: CPF, Email, Dinheiro, PropostaInfo
- **Enums**: StatusProposta, TipoSeguro

### SOLID
- **S**: Cada classe tem uma responsabilidade única
- **O**: Aberto para extensão, fechado para modificação
- **L**: Substituição de Liskov aplicada
- **I**: Interfaces segregadas por contexto
- **D**: Dependência de abstrações, não de implementações

### Design Patterns
- **Repository Pattern**: Acesso a dados abstrato
- **Use Case Pattern**: Casos de uso isolados
- **Factory Pattern**: Criação de objetos complexos
- **Strategy Pattern**: Diferentes tipos de seguro

## Validações Implementadas

### Value Objects
- **CPF**: Validação completa com dígitos verificadores
- **Email**: Validação de formato RFC compliant
- **Dinheiro**: Validação de valores positivos com precisão decimal

### Regras de Negócio
- Proposta criada sempre com status "Em Análise"
- Valor do prêmio deve ser menor que valor de cobertura
- Apenas propostas aprovadas podem ser contratadas
- Uma proposta pode ter apenas uma contratação
- Data de vigência não pode ser no passado
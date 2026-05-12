# BackendCsharp.API

Serviço de autenticação do desafio FullStack da Banana Ltda. Este backend em C# fica responsável por cadastro de usuários, login, hash de senha e emissão de JWT que será validado pelo backend Python de reservas.

## Responsabilidade na arquitetura

Este microsserviço não cuida de reservas. A responsabilidade dele é exclusivamente autenticação:

- cadastro de usuário
- login com validação de credenciais
- emissão de token JWT
- persistência dos dados de autenticação em banco relacional

O token emitido aqui é consumido pelo backend Python via header `Authorization: Bearer <token>`.

## Stack adotada

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- BCrypt.Net-Next para hash de senha
- JWT com assinatura simétrica
- FluentValidation para validação de entrada

## Fluxo de integração com o backend Python

1. O front-end envia `POST /api/user/login` para este serviço.
2. O backend C# autentica o usuário e retorna um JWT assinado.
3. O front-end encaminha esse JWT para o backend Python em todas as requisições protegidas.
4. O backend Python valida o token localmente com a mesma chave configurada por variável de ambiente.

## Endpoints

### `POST /api/user/register`

Cria um novo usuário.

Exemplo de payload:

```json
{
	"username": "user@email.com",
	"password": "Senha@123"
}
```

Retorno: usuário criado com `id`, `username` e `createdAt`.

### `POST /api/user/login`

Autentica o usuário e retorna o JWT.

Exemplo de payload:

```json
{
	"username": "user@email.com",
	"password": "Senha@123"
}
```

Retorno:

```json
{
	"token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### `GET /api/user`

Endpoint simples de health check/manual test.

## JWT gerado

O token inclui ao menos os seguintes claims:

- `sub`: identificador do usuário
- `email`: valor informado no cadastro/login
- `name`: nome de exibição do usuário
- `jti`: identificador único do token

## Variáveis de ambiente

O projeto usa a seção `Jwt` no `appsettings.json`, mas para o teste o ideal é sobrescrever esses valores por ambiente.

### Conexão com banco

- `ConnectionStrings__Default`

Exemplo:

```bash
ConnectionStrings__Default=Host=localhost;Port=5432;Database=csharp;Username=postgres;Password=admin
```

### JWT compartilhado com o backend Python

- `Jwt__Key`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__ExpiresInMinutes`

Exemplo:

```bash
Jwt__Key=super-secret-key-change-this-1234567890
Jwt__Issuer=auth-service
Jwt__Audience=reservation-service
Jwt__ExpiresInMinutes=60
```

Observação importante: a mesma `Jwt__Key` deve existir no backend Python para validação local do token.

## Como rodar localmente

### Pré-requisitos

- .NET SDK compatível com o projeto
- PostgreSQL em execução

### Instalação

```bash
dotnet restore
```

### Banco de dados

Se necessário, aplique as migrations do Entity Framework Core:

```bash
dotnet ef database update --project BackendCsharp.API/BackendCsharp.API.csproj
```

### Execução

```bash
dotnet run --project BackendCsharp.API/BackendCsharp.API.csproj
```

## Decisões técnicas

- BCrypt foi usado para não armazenar senha em texto puro.
- JWT foi escolhido por ser stateless e simples de integrar com o backend Python.
- Entity Framework Core foi usado por atender ao requisito de ORM e acelerar a persistência relacional.
- FluentValidation separa regra de validação da lógica de persistência.

## O que ainda pode ser evoluído

Este backend está funcional para autenticação, mas para ficar mais forte no contexto de avaliação técnica ainda há melhorias importantes:

1. Configurar de fato o pipeline de autenticação JWT no ASP.NET Core, com `UseAuthentication()` e parâmetros completos do bearer token.
2. Trocar o campo `username` para `email`, alinhando o contrato com o enunciado da vaga.
3. Implementar prevenção de cadastro duplicado por e-mail.
4. Adicionar refresh token, que é opcional no teste, mas seria um bom diferencial.
5. Cobrir o fluxo com testes automatizados de cadastro, login e geração do token.
6. Externalizar totalmente segredos e strings de conexão para variáveis de ambiente fora do `appsettings.json`.

## Observação final

O backend já está integrado com o serviço Python via JWT compartilhado, mas o contrato ainda pode ser refinado para ficar 100% aderente ao enunciado original da vaga.
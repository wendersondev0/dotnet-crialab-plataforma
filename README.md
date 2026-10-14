# dotnet-crialab-plataforma

API da Cria Lab DM. ASP.NET Core 8, vertical slice, SQL Server.

## Banco

Nesta máquina o SQL Server já escuta em `localhost` com autenticação do Windows. A connection string e a chave JWT ficam no user-secrets do projeto web, fora do git:

```bash
dotnet user-secrets set "ConnectionStrings:CriaLab" "Server=localhost;Database=crialab;Trusted_Connection=True;TrustServerCertificate=True" --project Presentation/CriaLab.Api.Presentation.WebApi
dotnet user-secrets set "Jwt:Key" "uma-chave-com-pelo-menos-32-caracteres" --project Presentation/CriaLab.Api.Presentation.WebApi
```

Com Docker, quando a porta 1433 estiver livre:

```bash
docker compose up -d
```

O SQL Server do compose usa `localhost,1433`, usuário `sa`, senha `CriaLab!Local_2026`, banco `crialab`.

## Migração

```bash
$env:CRIALAB_CONNECTION = "Server=localhost;Database=crialab;Trusted_Connection=True;TrustServerCertificate=True"
dotnet tool run dotnet-ef database update --project Infra/CriaLab.Api.Infra.SqlServer --startup-project Infra/CriaLab.Api.Infra.SqlServer
```

## API

```bash
dotnet run --project Presentation/CriaLab.Api.Presentation.WebApi
```

Sobe em `http://localhost:5088`. Saúde: `GET /health`.

Conta e loja: `POST /v1/auth/cadastro`, `POST /v1/auth/entrar`, `GET /v1/lojas/vitrine`. O administrador local usa o e-mail `crialab.dm@gmail.com` e a senha em `Admin:Senha` no user-secrets.

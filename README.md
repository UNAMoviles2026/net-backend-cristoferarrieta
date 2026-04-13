[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/QkpKJixH)
[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=23324774&assignment_repo_type=AssignmentRepo)
# Local Setup Guide

## Prerequisites

Install the following tools before running the project locally:

- .NET 8 SDK
- Docker

Before running the database container, make sure Docker Desktop (or the Docker daemon) is running.

## Run SQL Server (Docker)

Start a local SQL Server container with this command:

```bash
docker run -e "ACCEPT_EULA=Y" \
-e "SA_PASSWORD=YourStrong@Pass123" \
-p 1435:1433 \
--name sqlserver-lab \
-d mcr.microsoft.com/mssql/server:2022-latest
```

Use these database credentials:

- Username: `sa`
- Password: `YourStrong@Pass123`
- Port: `1435`

## Create Database

After the container is running, create a database named `reservations`.

```sql
CREATE DATABASE reservations;
```

You can run this command using any SQL client connected to your local SQL Server instance.

## Configure Connection String

Open `appsettings.json` and update the `ConnectionStrings` section so it points to your local SQL Server instance.

```json
"ConnectionStrings": {
	"DefaultConnection": "Server=localhost,1435;Database=reservations;User Id=sa;Password=YourStrong@Pass123;Encrypt=False;TrustServerCertificate=True;"
}
```

## Install Dependencies

Restore the project dependencies:

```bash
dotnet restore
dotnet tool restore
```

## Run Migrations

Restore the local Entity Framework CLI tool and apply the database schema:

```bash
dotnet tool restore
dotnet ef database update
```

What each command does:

- `dotnet tool restore`: restores the local EF Core CLI tool configured for the repository.
- `dotnet ef database update`: applies migrations to the `reservations` database.

This repository already contains migrations, so you only need `dotnet ef database update`.

## Run the Project

Start the API with:

```bash
dotnet run
```

After the API starts, Swagger will be available at:

```text
http://localhost:5147/swagger
```

## Useful Commands

Create project in root:

```bash
dotnet new webapi --output .
```

Add EF Core packages (version 8):

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

Create migration:

```bash
dotnet ef migrations add MigrationName
```

Update database:

```bash
dotnet ef database update
```

## Troubleshooting

- Docker container not running: verify the container is up with `docker ps` and start it if needed.
- Port 1433 already in use: map SQL Server to a different local port such as `1435` and update the connection string.
- Login failed for user 'sa': confirm the container is using `YourStrong@Pass123` and that the connection string matches it exactly.
- Invalid connection string: verify the server, database, username, password, `Encrypt=False`, and `TrustServerCertificate=True` values in `appsettings.json`.
- Missing migrations: run `dotnet ef migrations add InitialCreate` if no migration files exist yet, then run `dotnet ef database update`.

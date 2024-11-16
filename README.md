# Secret Book Club

## Description
A hobby project for my book club. C&C is welcome.

This project is built with the ideas of a layered "Onion" architecture as described by this link: 
https://code-maze.com/onion-architecture-in-aspnetcore/

## Prerequisites
- dotnet core 8
- docker
- entity framework cli tool

## Getting started
Assuming that all prerequisites are met, then running the following commands in the project root should get you started:
```bash
docker compose up -d
dotnet ef database update <MigrationName> --project Infrastructure/Persistence --startup-project Web
dotnet run --project web
```

## Database management.
This project uses entity framework with a code-first approach against a postgres database. A docker file is provided in 
the root to spin up a postgres instance.

Ensure that the Entity Framework CLI tool is installed:

```bash
dotnet tool install --global dotnet-ef
```

### Create a new migration
```bash
dotnet ef database update <MigrationName> --project Infrastructure/Persistence --startup-project Web
```

### Updating the Database

```bash
dotnet ef database update --project Infrastructure/Persistence --startup-project Web
```

### Rolling Back a Migration

```bash
dotnet ef database update <MigrationToRollbackTo> --project Infrastructure/Persistence --startup-project Web
```

### Remove Last Migration: 
If you need to delete the latest migration without applying it to the database, use:

```bash
dotnet ef migrations remove --project Infrastructure/Persistence --startup-project Web
```
## Tests
I'm a firm believer that tests should primarily test the contract of the application. This means that I'm more inclined 
toward integration tests than unit tests as that tests the "real" behaviour. The project leverages our docker setup 
together with the WebApplicationFactory provided by microsoft. This means that the integration test suite spins up a 
full application and sets up a database specifically for the tests. The database is recreated between each individual 
test.

Use the following command in the project root to run all tests:
```bash
dotnet test
```
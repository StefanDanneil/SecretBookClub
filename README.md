# Secret Book Club

This project uses Entity Framework Core with PostgreSQL for database management. Migrations are handled through the `Data` project to keep the database-related logic separate from the main API project.

## Prerequisites

Ensure that the Entity Framework CLI tool is installed:

```bash
dotnet tool install --global dotnet-ef
```

### Adding a migration

```bash
dotnet tool install --global dotnet-ef
```

### Updating the Database

```bash
dotnet ef database update --project Data --startup-project SecretBookClubApi
```

### Rolling Back a Migration

```bash
dotnet ef database update <MigrationName> --project Data --startup-project SecretBookClubApi
```

### Remove Last Migration: 
If you need to delete the latest migration without applying it to the database, use:

```bash
dotnet ef migrations remove --project Data --startup-project SecretBookClubApi
```
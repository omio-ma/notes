# Notes API

This repository contains a simple REST API for managing notes. It is built with .NET 9 following a layered architecture (API, Application, Domain and Infrastructure) and uses Entity Framework Core with SQL Server.

## Running locally

1. Install the .NET 9 SDK and ensure SQL Server LocalDB or a compatible SQL Server instance is available.
2. Create a `src/Notes.API/appsettings.Development.json` file with a connection string. Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NotesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Apply the database migrations:

```bash
dotnet ef database update --project src/Notes.Infrastructure --startup-project src/Notes.API
```

4. Start the API:

```bash
dotnet run --project src/Notes.API
```

By default the application listens on `https://localhost:7098` and `http://localhost:5145`. Use the `/notes` endpoints to create, read, update and delete notes.

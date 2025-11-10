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

## CI/CD workflows

The repository ships with reusable client and API pipelines plus a combined orchestration workflow (`.github/workflows/deploy.yml`).

* `client-ci.yml` builds and tests the React single-page application under `client/`.
* `dotnet-ci.yml` restores, builds and tests the .NET API.
* `deploy.yml` runs both CI workflows and, when requested, deploys the API to Azure App Service and the client to GitHub Pages.

### Running the deployment workflow safely

The deployment workflow runs automatically on pushes to `main`, but you can trigger it manually from the **Actions** tab using the **Run workflow** button. Manual runs expose two checkboxes:

* **Deploy the API to the configured Azure Web App**
* **Deploy the React client to GitHub Pages**

Disable either option to perform a dry run that only executes the tests and builds. This makes it easy to validate the pipeline from a feature branch without publishing new artifacts.

### Required secrets and hosting options

To publish successfully you must configure the following repository secrets:

* `SQL_SA_PASSWORD` – reused by the backend CI workflow for integration tests.
* `AZURE_WEBAPP_NAME` and `AZURE_WEBAPP_PUBLISH_PROFILE` – credentials for the Azure App Service instance that will host the API.

GitHub Pages covers the static SPA hosting for free. GitHub itself does not provide free hosting for .NET APIs, but the Azure App Service free tier works well with the included workflow. You can also adapt the deployment job to target other providers (for example, Azure Container Apps, Render, Fly.io) by replacing the `azure/webapps-deploy` step with the provider’s recommended GitHub Action.

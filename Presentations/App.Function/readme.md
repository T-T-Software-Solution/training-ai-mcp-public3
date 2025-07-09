# App.Function – Azure Functions MCP Tools

[⬅ Back to Solution Overview](../../readme.md)

This project implements Azure Functions for MCP (Model Context Protocol) tools, providing serverless endpoints for business operations such as customer, vehicle, service appointment, notification, stock, and quotation management.

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Azure Functions Core Tools ([docs](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local))
- SQL Server (local or remote instance)
- Access to required secrets (connection string, etc.)

---

## Configuration

### 1. `local.settings.json`

Configure your local development settings in `local.settings.json`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsSecretStorageType": "Files",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "DefaultConnection": "[Your connection string]"
  },
  "ConnectionStrings": {}
}
```

Replace `DefaultConnection` with your actual SQL Server connection string.

---

## Database Initialization

On startup, the function app ensures the database is created and seeds initial data using `DbInitializer.Seed`.

---

## Running Locally

From the `Presentations/App.Function` directory, run:

```powershell
func start
```

---

## Project Structure

- `Program.cs`: Main entry point, DI setup, and database seeding.
- `Tools/`: Azure Function classes for each business domain (Customer, Vehicle, ServiceAppointment, etc.).
- Uses services from `App.Database.Services`.

---

## References

- [Azure Functions .NET Worker](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide)
[⬅ Back to Solution Overview](../../readme.md)

# App.Mcp – MCP Server by Web API

App.Mcp is a presentation layer project that exposes the Model Context Protocol (MCP) server as a secure ASP.NET Core Web API.  
It provides endpoints for business operations such as customer, vehicle, service appointment, notification, stock, and quotation management, and integrates with Azure Entra ID for authentication.

This project is designed for scenarios where you want to host the MCP server as a traditional web API, enabling integration with web clients, external systems, or other services.

---

## Features

- **MCP Server**: Exposes business tools and operations via HTTP endpoints.
- **Secure Authentication**: Uses Azure Entra ID (Azure AD) for JWT-based authentication.
- **Database Integration**: Connects to SQL Server using Entity Framework Core.
- **Dependency Injection**: Registers all business services and tools for extensibility.
- **User Secrets Support**: Keeps sensitive configuration out of source control.

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Run all commands from the `Presentations\App.Mcp` directory or specify the project with `--project Presentations\App.Mcp`.

---

## 1. Initialize User Secrets

If you haven't already, initialize user secrets for the MCP project:

```powershell
dotnet user-secrets init
```

---

## 2. Store the Database Connection String

Replace the value with your actual connection string:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost\SQLEXPRESS;Database=DealershipSystem;Trusted_Connection=True;TrustServerCertificate=True"
```

---

## 3. Store Azure Entra ID (Azure AD) Settings

Replace the values in angle brackets with your actual Azure Entra ID details:

```powershell
dotnet user-secrets set "AzureAd:Instance" "https://login.microsoftonline.com/"
dotnet user-secrets set "AzureAd:Domain" "<your-tenant-name>.onmicrosoft.com"
dotnet user-secrets set "AzureAd:TenantId" "<your-tenant-id>"
dotnet user-secrets set "AzureAd:ClientId" "<your-api-app-client-id>"
dotnet user-secrets set "AzureAd:Audience" "api://<your-api-app-client-id>"
```

---

## 4. Remove Sensitive Values from appsettings.json

Your `appsettings.json` should keep the keys but not the actual secrets:

```json
"ConnectionStrings": {
  "DefaultConnection": ""
},
"AzureAd": {
  "Instance": "",
  "Domain": "",
  "TenantId": "",
  "ClientId": "",
  "Audience": ""
}
```

---

## 5. How It Works

- At runtime, ASP.NET Core will use values from user secrets (or environment variables) instead of `appsettings.json` if present.
- This keeps sensitive data out of your repository.

---

## 6. More Info

- [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
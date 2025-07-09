[⬅ Back to Solution Overview](../../readme.md)

# Secure Configuration with User Secrets

This project uses [ASP.NET Core User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to keep sensitive configuration values (like database connection strings and Azure Entra ID settings) out of source control.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Run all commands from the `Presentations\App.Api` directory or specify the project with `--project Presentations\App.Api`.

---

## 1. Initialize User Secrets

If you haven't already, initialize user secrets for the API project:

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
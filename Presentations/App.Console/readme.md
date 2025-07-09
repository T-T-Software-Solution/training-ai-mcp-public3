# App.Console – Azure Entra ID Authentication Sample

[⬅ Back to Solution Overview](../../readme.md)

This console app demonstrates how to authenticate with Azure Entra ID (Azure AD) using [Microsoft.Identity.Client (MSAL.NET)](https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-overview) and acquire an access token for calling a protected API.

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- An Azure Entra ID (Azure AD) tenant
- An app registration for this console app and for your API in Azure AD

---

## Configuration

### 1. `appsettings.json`

Add an `appsettings.json` file to your project root with the following content:

```json
{
  "AzureAd": {
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-console-app-client-id>",
    "ApiScope": "api://<your-api-app-client-id>/access_as_user",
    "RedirectUri": "http://localhost"
  }
}
```

Replace the placeholders with your actual Azure AD values.

---

### 2. (Recommended) Use User Secrets for Sensitive Data

To keep secrets out of source control, use [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

```powershell
dotnet user-secrets init
dotnet user-secrets set "AzureAd:TenantId" "<your-tenant-id>"
dotnet user-secrets set "AzureAd:ClientId" "<your-console-app-client-id>"
dotnet user-secrets set "AzureAd:ApiScope" "api://<your-api-app-client-id>/.default"
dotnet user-secrets set "AzureAd:RedirectUri" "http://localhost"
```

---

**Configure Azure OpenAI credentials**
   
   Store your Azure OpenAI endpoint, key, and deployment in user secrets:
   ```powershell
   dotnet user-secrets set "AzureOpenAI:Endpoint" "https://<your-endpoint>.openai.azure.com/"
   dotnet user-secrets set "AzureOpenAI:Key" "<your-key>"
   dotnet user-secrets set "AzureOpenAI:Deployment" "<your-deployment-name>"
   ```

## How It Works

- The app reads Azure AD settings from `appsettings.json` (overridden by user secrets if present).
- It uses MSAL.NET to acquire a token using the **authorization code flow** (interactive login).
- The access token can be used to call your protected API.

---

## Running the App

From the solution root or the `Presentations\App.Console` directory, run:

```powershell
dotnet run --project Presentations\App.Console
```

You should see the access token printed to the console after authenticating in your browser.

---

## References

- [Microsoft.Identity.Client documentation](https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-overview)
- [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Azure AD app registration quickstart](https://learn.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)

---
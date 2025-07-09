[⬅ Back to Solution Overview](../readme.md)

# Training Guide: Building and Understanding `CustomerFuncTool` in Azure Functions

This guide will help you (or your team) understand, build, and extend the `CustomerFuncTool` Azure Function from scratch. It covers the basics of the project structure, dependencies, and step-by-step instructions to create and test the main customer management functions.

---

## 1. Project Overview

- **Project Type:** Azure Functions (.NET 8, Isolated Worker)
- **Main Purpose:** Expose customer management operations (CRUD) as serverless functions.
- **Key File:** `CustomerFuncTool.cs` (in `Tools` folder)

---

## 2. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- SQL Server (local or cloud)
- Visual Studio Code (recommended) or Visual Studio

---

## 3. Solution Structure

```
App.Function/
│
├── Tools/
│   └── CustomerFuncTool.cs      # Main function logic
├── Program.cs                   # Host and DI setup
├── App_Function.csproj          # Project file with dependencies
└── ...
```

---

## 4. Step-by-Step: Creating `CustomerFuncTool` from Scratch

### Step 1: Create the Azure Functions Project

```sh
dotnet new func --name App.Function --worker-runtime dotnetIsolated --target-framework net8.0
```

### Step 2: Add Required NuGet Packages

Add these packages to your project using the following `dotnet` CLI commands (run from the `App.Function` project folder):

```sh
dotnet add package Microsoft.Azure.Functions.Worker
dotnet add package Microsoft.Azure.Functions.Worker.Extensions.Mcp --prerelease
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.ApplicationInsights.WorkerService
```

Add other packages as needed for your solution.

> **Note:** The `Microsoft.Azure.Functions.Worker.Extensions.Mcp` package is a prerelease version, so use the `--prerelease` flag to install it.

### Step 2.1: Add Project References via Command Line

To ensure your Azure Functions project can access the core business logic and database context, add the required project references using the following commands in your terminal (run from the `App.Function` project folder):

```sh
dotnet add reference ..\..\Infra\App.Database\App.Database.csproj
dotnet add reference ..\..\AppCore\App.AppCore\App.AppCore.csproj
```

This will update your `.csproj` file automatically and allow you to use the shared code

### Step 3: Set Up Dependency Injection and Database

In `Program.cs`, configure services and database context:

```csharp
// ...existing code...
builder.Services.AddDbContext<App.Database.AppContext>(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"]));
builder.Services.AddScoped<ICustomerService, CustomerService>();
// ...existing code...
```

### Step 4: Implement the Function Class

Create `Tools/CustomerFuncTool.cs`:

```csharp
// ...existing code...
public class CustomerFuncTool
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerFuncTool> _logger;

    public CustomerFuncTool(ICustomerService customerService, ILogger<CustomerFuncTool> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [Function(nameof(GetCustomerById))]
    public async Task<CustomerDto?> GetCustomerById(
        [McpToolTrigger("GetCustomerById", "...desc...")] ToolInvocationContext context,
        [McpToolProperty("id", "string", "...desc...")] string id,
        [McpToolProperty("userId", "string", "...desc...")] string? userId,
        [McpToolProperty("userRole", "string", "...desc...")] string? userRole)
    {
        if (!Guid.TryParse(id, out var guidId))
            return null;
        var customer = await _customerService.GetCustomerByIdAsync(guidId);
        if (customer == null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email
        };
    }
    // ...other methods (see full file)...
}
```

> **Tip:** Use the full code from your `CustomerFuncTool.cs` for all CRUD operations.

### Step 5: Build and Run Locally

```sh
dotnet build
func start
```

### Step 6: Test the Functions

- Use Azure Portal, Postman, or VS Code REST Client to trigger your functions.
- **Recommended:** Use the **MCP Inspector** tool to test your MCP-based Azure Functions easily.
- Check logs for output and errors.

> To run your function app locally, use:
>
> ```sh
> func start
> ```
>
> Then, use MCP Inspector to invoke and inspect your

---

## 5. Key Concepts

- **Dependency Injection:** Services like `ICustomerService` are injected for testability and separation of concerns.
- **Function Triggers:** `[McpToolTrigger]` and `[McpToolProperty]` attributes expose methods as callable endpoints.
- **DTOs:** Data Transfer Objects (`CustomerDto`) are used to shape output data.

---

## 6. Extending the Tool

- Add new methods for more customer operations.
- Implement validation and error handling.
- Integrate with other services (e.g., notifications).

---

## 7. Troubleshooting

- Ensure all dependencies are restored (`dotnet restore`).
- Check your connection string in `local.settings.json`.
- Use logs (`ILogger`) for debugging.

---

## 8. References

- [Azure Functions .NET Worker](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide)
- [Dependency Injection in Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

---

**You are ready to train your team on the `CustomerFuncTool` Azure Function!**
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using App.Database;
using App.Database.Services;
using App.AppCore.Interfaces;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.EnableMcpToolMetadata();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Add DbContext for MSSQL
builder.Services.AddDbContext<App.Database.AppContext>(options =>
    options.UseSqlServer(
        builder.Configuration["DefaultConnection"],
        sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    )
);

// Register business services for MCP tools
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IServiceAppointmentService, ServiceAppointmentService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<App.Database.AppContext>();
    context.Database.EnsureCreated();
    App.Database.DbInitializer.Seed(context);
}

app.Run();

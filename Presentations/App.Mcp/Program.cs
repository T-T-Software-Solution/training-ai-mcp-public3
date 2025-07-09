using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using App.Database;
using App.Database.Services;
using App.AppCore.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Register MCP server and discover tools from the current assembly
builder.Services.AddMcpServer().WithHttpTransport().WithToolsFromAssembly();

// Add DbContext for MSSQL
builder.Services.AddDbContext<App.Database.AppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register business services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IServiceAppointmentService, ServiceAppointmentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();

// Add authentication for Azure Entra ID
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add controller support
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Register IHttpContextAccessor (if you still have UserDetailsTools)
builder.Services.AddHttpContextAccessor();

// Map controllers
app.MapMcp().RequireAuthorization();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<App.Database.AppContext>();
    App.Database.DbInitializer.Seed(context);
}

app.Run();


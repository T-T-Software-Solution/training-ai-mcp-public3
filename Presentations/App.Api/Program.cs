using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using App.Database;
using App.Database.Services;
using App.AppCore.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Dealership API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your valid token in the text input below.\r\n\r\nExample: \"eyJhbGciOi...\""
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<App.Database.AppContext>();
    App.Database.DbInitializer.Seed(context);
}

app.Run();

using BackEnd.Application.Common.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure db
var connectionString = builder.Configuration.GetConnectionString("BackEndDb") ?? throw new InvalidOperationException("Connection string 'BackEndDb' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Only initialize database if not running for code generation (NSwag, etc.)
    var skipDbInit = builder.Configuration.GetValue<bool>("SkipDatabaseInitialization");
    if (!skipDbInit)
    {
        try
        {
            await app.InitialiseDatabaseAsync();
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(ex, "Database initialization failed. This is expected during build-time code generation.");
        }
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();

// Configure CORS
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentCors"); // More permissive for development
}
else
{
    app.UseCors("AllowFrontendApps"); // Strict policy for production
}

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseExceptionHandler(options => { });


app.MapEndpoints();

app.Run();

public partial class Program { }

using Scalar.AspNetCore;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// FORZAR Kestrel a escuchar solo en HTTP para evitar el cierre por falta de HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); 
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // UI moderna para ver tus APIs
}

app.MapGet("/", () => "Microservices users DevOps!");
app.MapGet("/health", () => "Healthy");


app.MapControllers();

app.Run();

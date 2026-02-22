using Scalar.AspNetCore;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Registrar capas de la aplicación
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // UI moderna para ver tus APIs
}

app.MapGet("/", () => "Microservices users DevOps!");
app.MapGet("/health", () => "Healthy");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

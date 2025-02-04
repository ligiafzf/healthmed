using HealthMed.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseCustomMiddleware();

app.Run();

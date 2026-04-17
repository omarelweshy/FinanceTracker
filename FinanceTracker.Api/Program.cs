using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

DatabaseMigrator.Run(connectionString);

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

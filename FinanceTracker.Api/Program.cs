using FinanceTracker.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseMigrator.Run(connectionString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.Run();
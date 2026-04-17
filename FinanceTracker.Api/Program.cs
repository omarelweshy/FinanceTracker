using FinanceTracker.Api.Endpoints;
using FinanceTracker.Application;
using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

DatabaseMigrator.Run(connectionString);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString, builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();

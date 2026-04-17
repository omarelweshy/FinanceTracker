using FinanceTracker.Api.Endpoints;
using FinanceTracker.Application;
using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Migrations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

DatabaseMigrator.Run(connectionString);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString, builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options => options
    .WithTitle("FinanceTracker API")
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
    .WithHttpBearerAuthentication(bearer => bearer.Token = string.Empty));

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();

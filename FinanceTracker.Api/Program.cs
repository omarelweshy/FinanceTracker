using FinanceTracker.Api.Endpoints;
using FinanceTracker.Api.Middleware;
using FinanceTracker.Application;
using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Migrations;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Host.UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration));

DatabaseMigrator.Run(connectionString);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString, builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference(options => options
    .WithTitle("FinanceTracker API")
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
    .WithHttpBearerAuthentication(bearer => bearer.Token = string.Empty));

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapAccountEndpoints();
app.MapCategoryEndpoints();

app.Run();

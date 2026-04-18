using System.Text;
using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Infrastructure.Messaging;
using FinanceTracker.Infrastructure.Repositories;
using FinanceTracker.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();

        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()!;
        services.AddSingleton(jwtSettings);
        services.AddScoped<ICategorySeeder, CategorySeeder>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenService, TokenService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

        services.AddAuthorization();

        var kafkaSettings = configuration.GetSection("Kafka").Get<KafkaSettings>() ?? new KafkaSettings();
        services.AddSingleton(kafkaSettings);
        services.AddSingleton<IEventBus, KafkaEventBus>();
        services.AddHostedService<KafkaConsumerService>();

        return services;
    }
}

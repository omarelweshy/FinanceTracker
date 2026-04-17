using DbUp;

namespace FinanceTracker.Infrastructure.Migrations;

public class DatabaseMigrator
{
    public static void Run(string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DatabaseMigrator).Assembly)
            .WithTransaction()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw new Exception($"Migration failed: {result.Error.Message}");
    }
}
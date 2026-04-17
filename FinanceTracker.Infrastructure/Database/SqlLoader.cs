namespace FinanceTracker.Infrastructure.Database;

public static class SqlLoader
{
    public static string Load(string name)
    {
        var assembly = typeof(SqlLoader).Assembly;
        var resourceName = $"FinanceTracker.Infrastructure.Database.Sql.{name}.sql";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"SQL resource not found: {resourceName}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

using System.Data;
using FinanceTracker.Application.Interfaces;

namespace FinanceTracker.Infrastructure.Database;

public class DbSession : IDbSession
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    public DbSession(IDbConnectionFactory factory)
    {
        _connection = factory.CreateConnection();
        _connection.Open();
    }

    public IDbConnection Connection => _connection;
    public IDbTransaction? Transaction => _transaction;

    public Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _transaction = _connection.BeginTransaction(isolationLevel);
        return Task.CompletedTask;
    }

    public Task CommitAsync()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _transaction?.Dispose();
        _connection.Dispose();
        return ValueTask.CompletedTask;
    }
}

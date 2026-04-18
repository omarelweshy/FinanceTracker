using System.Data;

namespace FinanceTracker.Application.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitAsync();
    Task RollbackAsync();
}

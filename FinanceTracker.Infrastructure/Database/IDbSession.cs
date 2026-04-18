using System.Data;
using FinanceTracker.Application.Interfaces;

namespace FinanceTracker.Infrastructure.Database;

public interface IDbSession : IUnitOfWork
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }
}

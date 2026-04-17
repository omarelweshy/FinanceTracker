using System.Data;

namespace FinanceTracker.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
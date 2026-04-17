using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TransferRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Transfer?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Transfer>(SqlLoader.Load("Transfers.GetById"), new { Id = id });
    }

    public async Task<IEnumerable<Transfer>> GetByUserIdAsync(Guid userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Transfer>(SqlLoader.Load("Transfers.GetByUserId"), new { UserId = userId });
    }

    public async Task AddAsync(Transfer transfer)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Transfers.Insert"), transfer);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Transfers.Delete"), new { Id = id });
    }
}

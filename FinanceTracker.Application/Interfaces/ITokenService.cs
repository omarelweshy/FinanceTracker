using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}

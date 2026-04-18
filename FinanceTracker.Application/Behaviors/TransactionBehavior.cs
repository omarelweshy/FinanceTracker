using FinanceTracker.Application.Common.Behaviors;
using FinanceTracker.Application.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    private readonly IUnitOfWork _uow;

    public TransactionBehavior(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync();
        try
        {
            var result = await next(cancellationToken);
            await _uow.CommitAsync();
            return result;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}

using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery(Guid UserId) : IRequest<UserDto>;

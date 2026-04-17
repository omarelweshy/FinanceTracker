using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new DomainException("User not found.");

        return new UserDto(user.Id, user.Email, user.FullName, user.Currency);
    }
}

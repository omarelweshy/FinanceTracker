using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public UpdateProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new DomainException("User not found.");

        user.UpdateProfile(request.FullName, request.Currency);
        await _userRepository.UpdateAsync(user);

        return new UserDto(user.Id, user.Email, user.FullName, user.Currency);
    }
}

using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new DomainException("Invalid email or password.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Invalid email or password.");

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(user.Id, user.Email, user.FullName, user.Currency, token);
    }
}

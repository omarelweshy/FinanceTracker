using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ICategorySeeder _categorySeeder;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
        ITokenService tokenService, ICategorySeeder categorySeeder)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _categorySeeder = categorySeeder;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email))
            throw new DomainException("Email is already registered.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, request.FullName, passwordHash, request.Currency);

        await _userRepository.AddAsync(user);
        await _categorySeeder.SeedAsync(user.Id);

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(user.Id, user.Email, user.FullName, user.Currency, token);
    }
}

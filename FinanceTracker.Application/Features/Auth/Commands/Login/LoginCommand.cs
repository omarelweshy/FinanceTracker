using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;

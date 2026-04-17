using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string FullName, string Password, string Currency) : IRequest<AuthResponse>;

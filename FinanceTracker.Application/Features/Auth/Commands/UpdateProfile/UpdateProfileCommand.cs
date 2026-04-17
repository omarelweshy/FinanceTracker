using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.UpdateProfile;

public record UpdateProfileCommand(Guid UserId, string FullName, string Currency) : IRequest<UserDto>;

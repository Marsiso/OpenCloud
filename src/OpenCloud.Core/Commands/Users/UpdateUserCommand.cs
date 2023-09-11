using MediatR;
using OpenCloud.Domain.Commands;

namespace OpenCloud.Core.Commands.Users;

public record UpdateUserCommand(int UserID, string? Identifier, string? FirstName, string? LastName, string? Email, string? ProfilePhotoUrl) : ICommand<Unit>;
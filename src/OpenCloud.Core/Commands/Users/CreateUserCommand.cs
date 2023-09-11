using MediatR;
using OpenCloud.Domain.Commands;

namespace OpenCloud.Core.Commands.Users;

public record CreateUserCommand(string? Identifier, string? FirstName, string? LastName, string? Email, string? ProfilePhotoUrl) : ICommand<Unit>;
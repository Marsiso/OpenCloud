using OpenCloud.Domain.Models;
using OpenCloud.Domain.Queries;

namespace OpenCloud.Core.Queries.Users;

public record GetUserByIdentifierQuery(string? Identifier) : IQuery<User?>;
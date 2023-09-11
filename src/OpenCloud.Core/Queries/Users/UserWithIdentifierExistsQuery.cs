using OpenCloud.Domain.Queries;

namespace OpenCloud.Core.Queries.Users;

public record UserWithIdentifierExistsQuery(string? Identifier) : IQuery<bool>;
using OpenCloud.Domain.Models;
using OpenCloud.Domain.Queries;

namespace OpenCloud.Core.Queries.Users;

public record GetUserByUserIDQuery(int UserID) : IQuery<User?>;
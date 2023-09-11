using Microsoft.EntityFrameworkCore;
using OpenCloud.Data;
using OpenCloud.Domain.Queries;

namespace OpenCloud.Core.Queries.Users;

public class UserWithIdentifierExistsQueryHandler : IQueryHandler<UserWithIdentifierExistsQuery, bool>
{
	private static readonly Func<DataContext, string, bool> _query = EF.CompileQuery((DataContext databaseContext, string identifier) =>
		databaseContext.Users.AsNoTracking().Any(user => user.Identifier == identifier));

	private readonly DataContext _databaseContext;

	public UserWithIdentifierExistsQueryHandler(DataContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<bool> Handle(UserWithIdentifierExistsQuery request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var userExists = false;

		if (string.IsNullOrWhiteSpace(request.Identifier)) return Task.FromResult(userExists);

		userExists = _query(_databaseContext, request.Identifier);

		return Task.FromResult(userExists);
	}
}
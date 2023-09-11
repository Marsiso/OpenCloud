using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCloud.Data;
using OpenCloud.Domain.Models;
using OpenCloud.Domain.Queries;

namespace OpenCloud.Core.Queries.Users;

public class GetUserByIdentifierQueryHandler : IQueryHandler<GetUserByIdentifierQuery, User?>
{
	private static readonly Func<DataContext, string, User?> _query = EF.CompileQuery((DataContext databaseContext, string identifier) =>
		databaseContext.Users.AsNoTracking().SingleOrDefault(user => user.Identifier == identifier));

	private readonly DataContext _databaseContext;
	private readonly ILogger<GetUserByIdentifierQueryHandler> _logger;

	public GetUserByIdentifierQueryHandler(DataContext databaseContext, ILogger<GetUserByIdentifierQueryHandler> logger)
	{
		_databaseContext = databaseContext;
		_logger = logger;
	}

	public Task<User?> Handle(GetUserByIdentifierQuery request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		if (string.IsNullOrEmpty(request.Identifier)) return Task.FromResult<User?>(default);

		var user = _query(_databaseContext, request.Identifier);

		return Task.FromResult(user);
	}
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCloud.Data;
using OpenCloud.Domain.Models;
using OpenCloud.Domain.Queries;

namespace OpenCloud.Core.Queries.Users;

public class GetUserByUserIDQueryHandler : IQueryHandler<GetUserByUserIDQuery, User?>
{
	private static readonly Func<DataContext, int, User?> _query = EF.CompileQuery((DataContext databaseContext, int userID) =>
		databaseContext.Users.AsNoTracking().SingleOrDefault(user => user.UserID == userID));

	private readonly DataContext _databaseContext;
	private readonly ILogger<GetUserByUserIDQueryHandler> _logger;

	public GetUserByUserIDQueryHandler(DataContext databaseContext, ILogger<GetUserByUserIDQueryHandler> logger)
	{
		_databaseContext = databaseContext;
		_logger = logger;
	}

	public Task<User?> Handle(GetUserByUserIDQuery request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		if (request.UserID < 1) return Task.FromResult<User?>(default);

		var user = _query(_databaseContext, request.UserID);

		return Task.FromResult(user);
	}
}
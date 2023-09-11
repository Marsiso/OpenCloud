using Microsoft.EntityFrameworkCore;
using OpenCloud.Domain.Models;

namespace OpenCloud.Data.Queries;

public static class UserPreCompiledQueries
{
	public static readonly Func<DataContext, int, User?> FindUserByUserID = EF.CompileQuery((DataContext databaseContext, int userID) =>
		databaseContext.Users.SingleOrDefault(user => user.UserID == userID));

	public static readonly Func<DataContext, string, User?> FindUserByIdentifier = EF.CompileQuery((DataContext databaseContext, string identifier) =>
		databaseContext.Users.SingleOrDefault(user => user.Identifier == identifier));

	public static readonly Func<DataContext, string, bool> UserWithIdentifierExists = EF.CompileQuery((DataContext databaseContext, string identifier) =>
		databaseContext.Users.Any(user => user.Identifier == identifier));
}
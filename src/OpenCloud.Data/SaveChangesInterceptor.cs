using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenCloud.Data.Queries;
using OpenCloud.Domain.Models;
using OpenCloud.Domain.Models.Common;

namespace OpenCloud.Data;

public class SaveChangesInterceptor : Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public SaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		if (eventData.Context is not DataContext databaseContext) return base.SavingChanges(eventData, result);

		var httpContext = _httpContextAccessor.HttpContext;

		OnBeforeSavedChanges(databaseContext, httpContext);

		return base.SavingChanges(eventData, result);
	}

	public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
	{
		if (eventData.Context is not DataContext databaseContext) return base.SavedChanges(eventData, result);

		var httpContext = _httpContextAccessor.HttpContext;

		OnAfterSavedChanges(databaseContext, httpContext);

		return base.SavedChanges(eventData, result);
	}

	private void OnBeforeSavedChanges(DataContext databaseContext, HttpContext? httpContext)
	{
		databaseContext.ChangeTracker.DetectChanges();

		var identifier = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		User? user = default;

		if (!string.IsNullOrWhiteSpace(identifier)) user = UserPreCompiledQueries.FindUserByIdentifier(databaseContext, identifier);

		var dateTime = DateTime.UtcNow;

		foreach (var entityEntry in databaseContext.ChangeTracker.Entries<ChangeTrackingEntity>())
			switch (entityEntry.State)
			{
				case EntityState.Added:
					entityEntry.Entity.IsActive = true;
					entityEntry.Entity.CreatedBy = entityEntry.Entity.UpdatedBy = user?.UserID;
					entityEntry.Entity.DateCreated = entityEntry.Entity.DateUpdated = dateTime;
					continue;
				case EntityState.Modified:
					entityEntry.Entity.UpdatedBy = user?.UserID;
					entityEntry.Entity.DateUpdated = dateTime;
					continue;
				case EntityState.Deleted:
					throw new InvalidOperationException($"Smazání záznamů v databázi není povolená operace. Služba: '{nameof(SaveChangesInterceptor)}' Akce: '{nameof(OnBeforeSavedChanges)}'.");
				default: continue;
			}
	}

	private void OnAfterSavedChanges(DataContext databaseContext, HttpContext? httpContext)
	{
	}
}
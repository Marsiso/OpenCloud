using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCloud.Domain.Models.Common;

namespace OpenCloud.Data.Configurations.Common;

public class ChangeTrackingEntityDatabaseMappingConfiguration<TEntity> : EntityBaseDatabaseMappingConfiguration<TEntity> where TEntity : ChangeTrackingEntity
{
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(entity => entity.DateCreated)
			.HasDefaultValueSql("datetime('now', 'localtime')")
			.ValueGeneratedOnAdd();

		builder.Property(entity => entity.DateUpdated)
			.HasDefaultValueSql("datetime('now', 'localtime')")
			.ValueGeneratedOnAddOrUpdate();

		builder.HasOne(entity => entity.UserCreatedBy)
			.WithMany()
			.HasForeignKey(entity => entity.CreatedBy)
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasOne(entity => entity.UserUpdatedBy)
			.WithMany()
			.HasForeignKey(entity => entity.UpdatedBy)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
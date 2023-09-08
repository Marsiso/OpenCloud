using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCloud.Domain.Models.Common;

namespace OpenCloud.Data.Configurations.Common;

public class EntityBaseDatabaseMappingConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasQueryFilter(e => e.IsActive);
	}
}
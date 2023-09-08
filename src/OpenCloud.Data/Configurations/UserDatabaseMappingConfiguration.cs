using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCloud.Data.Configurations.Common;
using OpenCloud.Domain.Models;

namespace OpenCloud.Data.Configurations;

public class UserDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<User>
{
	public override void Configure(EntityTypeBuilder<User> builder)
	{
		base.Configure(builder);

		builder.ToTable(Tables.Users);

		builder.HasKey(user => user.UserID);

		builder.HasIndex(user => user.Identifier);
		builder.HasIndex(user => user.IsActive);

		builder.HasIndex(user => user.Email)
			.IsUnique();

		builder.HasIndex(user => user.Identifier)
			.IsUnique();

		builder.Property(user => user.UserID)
			.IsRequired()
			.ValueGeneratedOnAdd();

		builder.Property(user => user.FirstName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(user => user.LastName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(user => user.Email)
			.IsRequired()
			.HasMaxLength(256);

		builder.Property(user => user.Identifier)
			.IsRequired()
			.HasMaxLength(512);

		builder.Property(user => user.ProfilePhotoUrl)
			.HasMaxLength(2048);
	}
}
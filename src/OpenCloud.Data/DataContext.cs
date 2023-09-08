using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenCloud.Domain.Models;

namespace OpenCloud.Data;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options, IOptions<DataContextOptions> providerOptions) : base(options)
	{
		_providerOptions = providerOptions;
	}

	private readonly IOptions<DataContextOptions> _providerOptions;

	public DbSet<User> Users { get; set; } = default!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		var source = Path.Combine(_providerOptions.Value.Location, _providerOptions.Value.FileName);

		var connectionStringBase = $"Data Source={source};";

		var connectionString = new SqliteConnectionStringBuilder(connectionStringBase)
		{
			Mode = SqliteOpenMode.ReadWriteCreate
		}.ToString();

		optionsBuilder.UseSqlite(connectionString);
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
	}
}
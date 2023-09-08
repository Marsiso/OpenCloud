using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace OpenCloud.Data;

public sealed class DataContext : DbContext
{
	private readonly IOptions<DataContextOptions> _providerOptions;

	public DataContext(DbContextOptions<DataContext> options, IOptions<DataContextOptions> providerOptions) : base(options)
	{
		_providerOptions = providerOptions;
	}

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
}
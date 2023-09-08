namespace OpenCloud.Data;

public class DataContextOptions
{
	public const string SectionName = "SQLite";

	public required string FileName { get; set; }
	public required string Location { get; set; }
}
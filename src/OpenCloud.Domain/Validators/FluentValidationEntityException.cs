namespace OpenCloud.Domain.Validators;

public class FluentValidationEntityException : Exception
{
	public FluentValidationEntityException(string entityTypeName, Dictionary<string, string[]> entityErrorsByProperty)
	{
		EntityTypeName = entityTypeName;
		EntityErrorsByProperty = entityErrorsByProperty;
	}

	public string EntityTypeName { get; }
	public Dictionary<string, string[]> EntityErrorsByProperty { get; }
}
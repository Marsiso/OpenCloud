namespace OpenCloud.Domain.Validators;

public class FluentValidationEntityException : Exception
{
	public FluentValidationEntityException(Dictionary<string, string[]> entityErrorsByProperty)
	{
		EntityErrorsByProperty = entityErrorsByProperty;
	}

	public Dictionary<string, string[]> EntityErrorsByProperty { get; set; }
}
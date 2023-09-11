namespace OpenCloud.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
	public EntityNotFoundException(int entityID, string entityTypeName)
	{
		EntityID = entityID;
		EntityTypeName = entityTypeName;
	}

	public int EntityID { get; set; }
	public string EntityTypeName { get; set; }
}
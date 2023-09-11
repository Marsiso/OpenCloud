namespace OpenCloud.Domain.Enums;

public enum DatabaseOperationOutcome
{
	Success,
	InternalServerError,
	EntityNotFound,
	ReferenceConstraintViolation,
	UniqueConstraintViolation
}
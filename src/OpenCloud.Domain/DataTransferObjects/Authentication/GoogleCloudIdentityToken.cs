namespace OpenCloud.Domain.DataTransferObjects.Authentication;

public record GoogleCloudIdentityToken(string? Identifier, string? FirstName, string? LastName, string? Email, string? ProfilePhotoUrl);
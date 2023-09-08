using OpenCloud.Domain.Models.Common;

namespace OpenCloud.Domain.Models;

public class User : ChangeTrackingEntity
{
	public int UserID { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Identifier { get; set; } = string.Empty;
	public string? ProfilePhotoUrl { get; set; }
}
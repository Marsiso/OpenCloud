using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace OpenCloud.Application.Authentication;

public class BlazorAuthenticationStateProvider : AuthenticationStateProvider
{
	public BlazorAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	private readonly IHttpContextAccessor _httpContextAccessor;

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var httpContext = _httpContextAccessor.HttpContext;

			var claimsPrincipal = httpContext?.User;

			if (claimsPrincipal is not { Claims: not null } || !claimsPrincipal.Claims.Any())
			{
				return Task.FromResult(GetAnonymousAuthenticationState());
			}
			else
			{
				var identifier = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
				var firstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);
				var lastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname);
				var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
				var profilePhotoUrl = claimsPrincipal.FindFirstValue("urn:google:image");

				return Task.FromResult(new AuthenticationState(claimsPrincipal));
			}
		}
		catch (Exception)
		{
			return Task.FromResult(GetAnonymousAuthenticationState());
		}
	}

	private static AuthenticationState GetAnonymousAuthenticationState()
	{
		return new AuthenticationState(user: new ClaimsPrincipal(identity: new ClaimsIdentity()));
	}
}
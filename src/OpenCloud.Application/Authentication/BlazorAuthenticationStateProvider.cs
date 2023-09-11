using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using OpenCloud.Core.Commands.Users;
using OpenCloud.Core.Queries.Users;

namespace OpenCloud.Application.Authentication;

public class BlazorAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IMapper _mapper;
	private readonly ISender _messageHandlerBroker;

	public BlazorAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, ISender messageHandlerBroker, IMapper mapper)
	{
		_httpContextAccessor = httpContextAccessor;
		_messageHandlerBroker = messageHandlerBroker;
		_mapper = mapper;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var httpContext = _httpContextAccessor.HttpContext;

			var claimsPrincipal = httpContext?.User;

			if (claimsPrincipal is not { Claims: not null } || !claimsPrincipal.Claims.Any())
			{
				return GetAnonymousAuthenticationState();
			}
			else
			{
				var identifier = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
				var firstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);
				var lastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname);
				var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
				var profilePhotoUrl = claimsPrincipal.FindFirstValue("urn:google:image");

				var userExistsQuery = new UserWithIdentifierExistsQuery(identifier);

				var userExists = await _messageHandlerBroker.Send(userExistsQuery);

				if (userExists)
				{
					var getUserQuery = new GetUserByIdentifierQuery(identifier);

					var originalUser = await _messageHandlerBroker.Send(getUserQuery);

					if (originalUser is null) return GetAnonymousAuthenticationState();

					var updateUserCommand = _mapper.Map<UpdateUserCommand>(originalUser);

					updateUserCommand = updateUserCommand with
					{
						FirstName = firstName,
						LastName = lastName,
						Email = email,
						ProfilePhotoUrl = profilePhotoUrl
					};

					await _messageHandlerBroker.Send(updateUserCommand);
				}
				else
				{
					var createUserCommand = new CreateUserCommand(identifier, firstName, lastName, email, profilePhotoUrl);

					await _messageHandlerBroker.Send(createUserCommand);
				}

				return new AuthenticationState(claimsPrincipal);
			}
		}
		catch (Exception)
		{
			return GetAnonymousAuthenticationState();
		}
	}

	private static AuthenticationState GetAnonymousAuthenticationState()
	{
		return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
	}
}
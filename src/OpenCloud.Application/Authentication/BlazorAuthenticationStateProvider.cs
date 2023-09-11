using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using OpenCloud.Core.Commands.Users;
using OpenCloud.Core.Queries.Users;
using OpenCloud.Domain.DataTransferObjects.Authentication;

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

			if (claimsPrincipal is not { Claims: not null } || !claimsPrincipal.Claims.Any()) return GetAnonymousAuthenticationState();

			var authenticationToken = new GoogleCloudIdentityToken(
				claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
				claimsPrincipal.FindFirstValue(ClaimTypes.GivenName),
				claimsPrincipal.FindFirstValue(ClaimTypes.Surname),
				claimsPrincipal.FindFirstValue(ClaimTypes.Email),
				claimsPrincipal.FindFirstValue("urn:google:image"));

			var userExistsQuery = new UserWithIdentifierExistsQuery(authenticationToken.Identifier);

			var userExists = await _messageHandlerBroker.Send(userExistsQuery);

			if (userExists)
			{
				var getUserQuery = new GetUserByIdentifierQuery(authenticationToken.Identifier);

				var originalUser = await _messageHandlerBroker.Send(getUserQuery);

				if (originalUser is null) return GetAnonymousAuthenticationState();

				_ = _mapper.Map(authenticationToken, originalUser);

				var updateUserCommand = _mapper.Map<UpdateUserCommand>(originalUser);

				_ = await _messageHandlerBroker.Send(updateUserCommand);
			}
			else
			{
				var createUserCommand = _mapper.Map<CreateUserCommand>(authenticationToken);

				_ = await _messageHandlerBroker.Send(createUserCommand);
			}

			return new AuthenticationState(claimsPrincipal);
		}
		catch (Exception exception)
		{
			return GetAnonymousAuthenticationState();
		}
	}

	private static AuthenticationState GetAnonymousAuthenticationState()
	{
		return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
	}
}
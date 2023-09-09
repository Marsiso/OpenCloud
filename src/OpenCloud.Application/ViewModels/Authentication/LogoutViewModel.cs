﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenCloud.Application.ViewModels.Authentication;

public class LogoutViewModel : PageModel
{
	public string? ReturnUrl { get; }

	public async Task<IActionResult> OnGetAsync(string? returnUrl = default)
	{
		returnUrl ??= Url.Content($"~{Routes.Index}");

		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		return LocalRedirect(Routes.Index);
	}
}
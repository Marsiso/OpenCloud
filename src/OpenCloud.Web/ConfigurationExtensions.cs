using Cloud.Application.Validations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenCloud.Application.Authentication;
using OpenCloud.Application.Validators;
using OpenCloud.Core.Commands.Users;
using OpenCloud.Data;
using SaveChangesInterceptor = OpenCloud.Data.SaveChangesInterceptor;

namespace OpenCloud.Web;

public static class ConfigurationExtensions
{
	public static IServiceCollection AddSqlite(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
	{
		services.AddSingleton<IValidator<DataContextOptions>, DataContextOptionsValidator>();

		services
			.AddOptions<DataContextOptions>()
			.Bind(configuration.GetSection(DataContextOptions.SectionName))
			.ValidateFluently()
			.ValidateOnStart();

		services
			.AddHttpContextAccessor()
			.AddScoped<HttpContextAccessor>();

		services.AddScoped<ISaveChangesInterceptor, SaveChangesInterceptor>();

		services.AddDbContext<DataContext>(options =>
		{
			options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

			if (environment.IsDevelopment())
			{
				options.EnableDetailedErrors();
				options.EnableSensitiveDataLogging();
			}
		});

		return services;
	}

	public static IServiceCollection AddGoogleCloudIdentity(this IServiceCollection services, IConfiguration configuration)
	{
		var googleCloudIdentityConfigurationSection = configuration.GetSection(GoogleCloudIdentityOptions.SegmentName);

		ArgumentNullException.ThrowIfNull(googleCloudIdentityConfigurationSection);

		var googleCloudIdentityOptions = googleCloudIdentityConfigurationSection.Get<GoogleCloudIdentityOptions>();

		ArgumentNullException.ThrowIfNull(googleCloudIdentityOptions);

		services.AddSingleton<IValidator<GoogleCloudIdentityOptions>, GoogleCloudIdentityOptionsValidator>();

		services
			.AddOptions<GoogleCloudIdentityOptions>()
			.Bind(googleCloudIdentityConfigurationSection)
			.ValidateFluently()
			.ValidateOnStart();

		services
			.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie();

		services
			.AddAuthentication()
			.AddGoogle(options =>
			{
				options.ClientId = googleCloudIdentityOptions.ClientID;
				options.ClientSecret = googleCloudIdentityOptions.ClientSecret;
				options.CallbackPath = googleCloudIdentityOptions.CallbackPath;
				options.ClaimActions.MapJsonKey("urn:google:profile", "link");
				options.ClaimActions.MapJsonKey("urn:google:image", "picture");

				options.SaveTokens = true;
			});

		services.AddHttpContextAccessor();
		services.AddScoped<HttpContextAccessor>();

		services.AddHttpClient();
		services.AddScoped<HttpClient>();

		services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();

		return services;
	}

	public static IServiceCollection AddCQRS(this IServiceCollection services)
	{
		services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));

		services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehaviour<,>));

		return services;
	}
}
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenCloud.Application.Validators;
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
}
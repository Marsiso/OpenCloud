using MudBlazor.Services;
using OpenCloud.Application.Mappings;
using OpenCloud.Application.Security;
using OpenCloud.Data;
using OpenCloud.Web;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddRazorPages();
applicationBuilder.Services.AddServerSideBlazor();

applicationBuilder.Services.AddMudServices();

applicationBuilder.Services.AddAutoMapper(typeof(UserMappingConfiguration));
applicationBuilder.Services.AddSqlite(applicationBuilder.Configuration, applicationBuilder.Environment);
applicationBuilder.Services.AddGoogleCloudIdentity(applicationBuilder.Configuration);
applicationBuilder.Services.AddCQRS();

var application = applicationBuilder.Build();

using var serviceScope = application.Services.CreateScope();

var databaseContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

if (application.Environment.IsDevelopment())
{
	application.UseExceptionHandler("/Error");

	databaseContext.Database.EnsureDeleted();
	databaseContext.Database.EnsureCreated();
}
else
{
	application.UseSecurityHeaders(SecurityHeaderHelpers.GetHeaderPolicyCollection());

	databaseContext.Database.EnsureCreated();
}

application.UseHttpsRedirection();

application.UseStaticFiles();

application.UseCookiePolicy();
application.UseAuthentication();

application.UseRouting();

application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();
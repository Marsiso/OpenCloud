using OpenCloud.Data;
using OpenCloud.Web;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddRazorPages();
applicationBuilder.Services.AddServerSideBlazor();

applicationBuilder.Services.AddSqlite(applicationBuilder.Configuration, applicationBuilder.Environment);

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
	databaseContext.Database.EnsureCreated();
}

application.UseHttpsRedirection();

application.UseStaticFiles();

application.UseRouting();

application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();
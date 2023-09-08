var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddRazorPages();
applicationBuilder.Services.AddServerSideBlazor();

var application = applicationBuilder.Build();

application.UseHttpsRedirection();

application.UseStaticFiles();

application.UseRouting();

application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();
using IMSWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);
var CookieScheme = "IMSWebApp";

//builder.Services.AddDevExpressControls();
builder.Services.AddMvc();

builder.Services.AddResponseCaching();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieScheme;
}
)
.AddCookie(CookieScheme, options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.AccessDeniedPath = "/Denied";
    options.LoginPath = "/Login";
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IGlobalService, GlobalService>();

//builder.Services.ConfigureReportingServices(configurator => {
//    configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
//        viewerConfigurator.UseCachedReportSourceBuilder();
//    });
//    configurator.DisableCheckForCustomControllers();
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "node_modules")),
//    RequestPath = "/node_modules"
//});

//app.UseDevExpressControls();

app.UseResponseCaching();
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}");  // Default route for MVC
});


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}");

app.Run();

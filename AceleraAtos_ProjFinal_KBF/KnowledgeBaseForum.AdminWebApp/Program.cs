using KnowledgeBaseForum.AdminWebApp.Services;
using KnowledgeBaseForum.Commons.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddConfig(config).AddScopedDepedencies();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(480);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Const.ROLE_ADMIN, policy => policy.RequireClaim(Const.ROLE_ADMIN));
    options.AddPolicy(Const.ROLE_NORMAL, policy => policy.RequireClaim(Const.ROLE_NORMAL));
});

var app = builder.Build();
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
};

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseCookiePolicy(cookiePolicyOptions);
app.Run();

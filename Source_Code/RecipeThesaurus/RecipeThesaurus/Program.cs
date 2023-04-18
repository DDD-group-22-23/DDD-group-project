using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;

/*
//Use this for sqlite
using RecipeThesaurus;

sqlite a = new sqlite();
a.run();
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.SignInScheme = "Cookies";

        options.ClientId = "75dc9e9f-ee9b-4baf-afb5-cde509ee94d7";
        options.ClientSecret = "JF2rzNTNHUEIwV5O1vshA6s5q4wGhCoBMff7w9DpPR0";
        options.Authority = "http://backoffice.recipethesaurus.software:9011/.well-known/openid-configuration/13bbefea-b1d0-11ed-9c51-6adf74905334";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;

        options.ResponseType = "code";

        options.Scope.Add("profile");
        options.Scope.Add("offline");
        options.SaveTokens = true;
    })
    ;

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


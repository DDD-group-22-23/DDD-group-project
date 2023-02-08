using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecipeThesaurus.Models.DB;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

builder.Services.AddRazorPages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// This method gets called by the runtime. Use this method to add services to the container.  
// [Asma Khalid]: Authorization settings.  
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(options =>
    {
        options.LoginPath = new PathString("/Index");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
    });

// [Asma Khalid]: Authorization settings.  
builder.Services.AddMvc().AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeFolder("/");
        options.Conventions.AllowAnonymousToPage("/Index");
    });

// [Asma Khalid]: Register SQL database configuration context as services.    
builder.Services.AddDbContext<RecipeThesaurusContext>(options => builder.Configuration.GetConnectionString("RecipeThesaurus"));
app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();

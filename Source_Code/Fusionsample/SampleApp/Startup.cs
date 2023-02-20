using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace SampleApp
{
    public class Startup
    {
 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();                
            });

            ILogger logger = loggerFactory.CreateLogger<Startup>();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // Configure your policies
            services.AddAuthorization(options =>
                options.AddPolicy("Registered", policy => policy.RequireClaim("aud", Configuration["SampleApp:ClientId"]))
            );
            services.AddAuthorization(options =>
                options.AddPolicy("Registered", policy => policy.RequireClaim("iss", Configuration["SampleApp:Authority"]))
            );
            //services.AddAuthorization(options =>
                //options.AddPolicy("Registered", policy => policy.RequireClaim("userType", "premium"))
            //);

            services.AddRazorPages();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = Configuration["SampleApp:CookieName"];
                })
                .AddOpenIdConnect("oidc", options =>
                {

                    options.Authority = Configuration["SampleApp:Authority"];
                    options.ClientId = Configuration["SampleApp:ClientId"];
                    options.ClientSecret = Configuration["SampleApp:ClientSecret"];
                    options.Scope.Add("openid");

                    // leave this in, otherwise the aud claim is removed. See https://stackoverflow.com/questions/69289426/missing-aud-claim-in-asp-net-core-policy-check-prevents-authorization-but-it for more
                    options.ClaimActions.Remove("aud");
                    options.ClaimActions.Remove("iss");

                    options.ResponseType = "code";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = Configuration["SampleApp:ClientId"]
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
	    IdentityModelEventSource.ShowPII = true;
        }
    }
}


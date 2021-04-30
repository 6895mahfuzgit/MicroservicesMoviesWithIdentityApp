using IdentityModel;
using IdentityModel.Client;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;
using System;

namespace Movies.Client
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
            services.AddControllersWithViews();
            services.AddScoped<IMovieApiService, MovieApiService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
             {
                 options.Authority = Configuration.GetValue<string>("IdentityServer:ServerURL");
                 options.ClientId = Configuration.GetValue<string>("IdentityServer:ClientId");
                 options.ClientSecret = Configuration.GetValue<string>("IdentityServer:ClientSecret");
                 options.ResponseType = Configuration.GetValue<string>("IdentityServer:ResponseType");
                 // options.Scope.Add(Configuration.GetValue<string>("IdentityServer:ScopeOption1"));
                 //options.Scope.Add(Configuration.GetValue<string>("IdentityServer:ScopeOption2"));
                 options.Scope.Add(Configuration.GetValue<string>("IdentityServer:ScopeOption3"));
                 options.Scope.Add(Configuration.GetValue<string>("IdentityServer:ScopeOption4"));
                 options.Scope.Add(Configuration.GetValue<string>("IdentityServer:ScopeOption5"));
                 options.Scope.Add(Configuration.GetValue<string>("IdentityServer:ScopeOption6"));

                 options.ClaimActions.MapUniqueJsonKey("role","role");
                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;

                 options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                 {
                     NameClaimType = JwtClaimTypes.GivenName,
                     RoleClaimType = JwtClaimTypes.Role
                 };
             });


            //Create Client
            services.AddTransient<AuthenticationDelegateHandler>();
            services.AddHttpClient("MovieAPIClient", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("APIServer:ServerURL"));
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            }).AddHttpMessageHandler<AuthenticationDelegateHandler>();

            services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("IdentityServer:ServerURL"));
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            });

            services.AddHttpContextAccessor();
            //services.AddSingleton(new ClientCredentialsTokenRequest
            //{
            //    Address = $"{Configuration.GetValue<string>("IdentityServer:ServerURL")}/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = Configuration.GetValue<string>("IdentityServer:ClientSecret"),
            //    Scope = Configuration.GetValue<string>("IdentityServer:ScopeOption3")
            //});

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

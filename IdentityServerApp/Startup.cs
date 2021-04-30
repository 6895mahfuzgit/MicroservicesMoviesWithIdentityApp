using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //for identity Server
            services.AddIdentityServer()
                     .AddInMemoryClients(Config.Clients)
                     //.AddInMemoryIdentityResources(Config.IdentityResources)
                     //.AddInMemoryApiResources(Config.ApiResources)
                     .AddInMemoryApiScopes(Config.ApiScopes)
                     .AddInMemoryIdentityResources(Config.IdentityResources)
                    //.AddTestUsers(Config.TestUsers)
                    .AddTestUsers(TestUsers.Users)
                     .AddDeveloperSigningCredential();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            //for identity Server
            app.UseIdentityServer();
            app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}

using IdentityServer4.EntityFramework.Mappers;
using IdentityServerApp.Context;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace IdentityServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();

            //for identity Server
            //services.AddIdentityServer()
            //         .AddInMemoryClients(Config.Clients)
            //         //.AddInMemoryIdentityResources(Config.IdentityResources)
            //         //.AddInMemoryApiResources(Config.ApiResources)
            //         .AddInMemoryApiScopes(Config.ApiScopes)
            //         .AddInMemoryIdentityResources(Config.IdentityResources)
            //        //.AddTestUsers(Config.TestUsers)
            //        .AddTestUsers(TestUsers.Users)
            //         .AddDeveloperSigningCredential();
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            services.AddIdentityServer()
                    .AddTestUsers(TestUsers.Users)
                    .AddConfigurationStore(options =>
                        {
                            options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                        })
                  .AddOperationalStore(options =>
                        {
                            options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                        });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitializeDatabase(app);
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

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}

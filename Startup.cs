using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NameDirectoryService.Models;
using Microsoft.EntityFrameworkCore;
using NameDirectoryService.DAL;
using Microsoft.Extensions.Options;

namespace NameDirectoryService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            Action<ConnectionSettings> conf = (c) => {
                c.DriverClassName = Configuration["database_driverClassName"];
                c.DefaultConnection = Configuration["database_url"];
            };

            if(Configuration["database_driverClassName"] != null && Configuration["database_url"] != null )
                services.Configure<ConnectionSettings>(conf); // configure by environment variables
            else
                services.Configure<ConnectionSettings>(Configuration.GetSection("ConnectionStrings")); // configure by appsettings.json
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<ConnectionSettings> settings)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigureDatabase(env, loggerFactory, settings);
        }

        private void ConfigureDatabase(IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<ConnectionSettings> settings)
        {
            var logger = loggerFactory.CreateLogger("Logger");
            try
            {
                //using (var db = new NameDirectoryDbContext(settings, env.WebRootPath, logger))
                //{
                //    db.Database.Migrate();
                //}
            }
            catch (Exception ex)
            {
                logger.LogError("ConfigureDatabase error: " + ex.Message);
            }
        }
    }
}

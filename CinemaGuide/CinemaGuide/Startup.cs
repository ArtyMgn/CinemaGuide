using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CinemaGuide.Models;
using CinemaGuide.Models.MoviesDatabases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaGuide
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            
            var aggregatorConfig = new AggregatorConfig();
            Configuration.GetSection("AggregatorSettings").Bind(aggregatorConfig);

            containerBuilder.RegisterInstance(aggregatorConfig).SingleInstance();
            containerBuilder.RegisterType<Tmdb>().As<IMovieDatabase>();
            containerBuilder.RegisterType<MoviesAggregator>().SingleInstance();
            
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("Home/Error");
            }
        
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/");
            });
        }
    }
}

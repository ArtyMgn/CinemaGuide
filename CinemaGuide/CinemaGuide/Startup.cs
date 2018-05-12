using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CinemaGuide.Api;
using CinemaGuide.Models;
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
            var settingsPath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? "appsettings.Development.json"
                : "appsettings.json";

            var builder = new ConfigurationBuilder().SetBasePath(hostingEnvironment.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("tokens.json", true, true)
                .AddJsonFile(settingsPath,  true, true);

            Configuration = builder.Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = c => c.Context.Response.Headers["Cache-Control"] = "public,max-age=1"
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseStaticFiles();
            }

            app.UseStaticFiles();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/"); });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            var apiTypes = RegisterApi(containerBuilder);
            RegisterDefaultProfile(containerBuilder, apiTypes);

            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        private List<Type> RegisterApi(ContainerBuilder containerBuilder)
        {
            var apiType = typeof(ICinemaApi);

            var apiTypes = apiType.Assembly.GetTypes()
                .Where(p => p.GetInterfaces().Contains(apiType))
                .ToList();

            var paramName = apiTypes[0]
                .GetConstructor(new[] { typeof(string) })
                .GetParameters()[0]
                .Name;

            var tokens = new Dictionary<string, string>();

            Configuration.GetSection("Tokens").Bind(tokens);

            foreach (var type in apiTypes)
            {
                containerBuilder
                    .RegisterType(type)
                    .WithParameter(paramName, tokens[type.Name])
                    .As(apiType)
                    .SingleInstance();
            }

            return apiTypes;
        }

        private void RegisterDefaultProfile(ContainerBuilder containerBuilder, List<Type> apiTypes)
        {
            var profile              = new Profile { SourceList = apiTypes };
            var profileType          = profile.GetType();
            var registrationsBuilder = containerBuilder.RegisterType(profileType);

            Configuration.GetSection("DefaultProfile").Bind(profile);

            foreach (var property in profileType.GetProperties())
            {
                registrationsBuilder.WithProperty(property.Name, property.GetValue(profile));
            }
        }
    }
}

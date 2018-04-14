﻿using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CinemaGuide.Api;
using CinemaGuide.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                .AddEnvironmentVariables() // ?
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
            }
            else
            {
                app.UseExceptionHandler("Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/"); });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            var tokens         = new Tokens();
            var defaultProfile = new Profile();

            Configuration.GetSection("Tokens").Bind(tokens);
            Configuration.GetSection("DefaultProfile").Bind(defaultProfile);

            RegisterProfile(containerBuilder, defaultProfile);
            RegisterApi(containerBuilder, tokens);

            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        private static void RegisterApi(ContainerBuilder containerBuilder, Tokens tokens)
        {
            var apiType = typeof(ICinemaApi);

            var apiTypes = apiType.Assembly.GetTypes()
                .Where(p => p.GetInterfaces().Contains(apiType))
                .ToList();

            var paramName = apiTypes[0]
                .GetConstructor(new[] { typeof(string) })
                .GetParameters()[0]
                .Name;

            foreach (var type in apiTypes)
            {
                containerBuilder
                    .RegisterType(type)
                    .WithParameter(paramName, tokens.GetType().GetProperty(type.Name).GetValue(tokens))
                    .As(apiType);
            }
        }

        private static void RegisterProfile(ContainerBuilder containerBuilder, Profile profile)
        {
            var profileType          = profile.GetType();
            var registrationsBuilder = containerBuilder.RegisterType(profileType);

            foreach (var property in profileType.GetProperties())
            {
                registrationsBuilder.WithProperty(property.Name, property.GetValue(profile));
            }
        }
    }
}

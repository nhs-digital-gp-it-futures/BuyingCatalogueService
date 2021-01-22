using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHSD.BuyingCatalogue.API.Extensions;
using NHSD.BuyingCatalogue.API.Infrastructure;
using NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.API.Infrastructure.Logging;
using NHSD.BuyingCatalogue.Capabilities.Application;
using NHSD.BuyingCatalogue.Capabilities.Application.Mapping;
using NHSD.BuyingCatalogue.Capabilities.Contracts;
using NHSD.BuyingCatalogue.Capabilities.Persistence;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.SolutionLists.Application;
using NHSD.BuyingCatalogue.SolutionLists.Application.Mapping;
using NHSD.BuyingCatalogue.SolutionLists.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application;
using NHSD.BuyingCatalogue.Solutions.Application.Mapping;
using NHSD.BuyingCatalogue.Solutions.Persistence;
using Serilog;

namespace NHSD.BuyingCatalogue.API
{
    /// <summary>
    /// Represents a boot strapper for the application. Used as a starting point to configure the API.
    /// </summary>
    public sealed class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.GetAssembly(typeof(SolutionAutoMapperProfile)),
                Assembly.GetAssembly(typeof(SolutionListAutoMapperProfile)),
                Assembly.GetAssembly(typeof(CapabilityAutoMapperProfile)),
                Assembly.GetAssembly(typeof(ICapability)),
            };

            var settings = new Settings(config);

            services
                .AddSingleton<ISettings>(settings)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
                .AddAutoMapper(assemblies)
                .AddMediatR(assemblies)
                .RegisterSolutionApplication()
                .RegisterSolutionListApplication()
                .RegisterCapabilitiesApplication()
                .RegisterData()
                .RegisterSolutionsPersistence()
                .RegisterCapabilityPersistence()
                .RegisterSolutionListPersistence()
                .RegisterHealthChecks(settings)
                .AddCustomSwagger()
                .AddCustomMvc();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment details.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging(opts =>
            {
                opts.GetLevel = SerilogRequestLoggingOptions.GetLevel;
            });

            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger()
                   .UseSwaggerUI(options =>
                  {
                      options.SwaggerEndpoint("/swagger/v1/swagger.json", "Buying Catalogue API V1");
                  });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = healthCheckRegistration => healthCheckRegistration.Tags.Contains(HealthCheckTags.Live),
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = healthCheckRegistration => healthCheckRegistration.Tags.Contains(HealthCheckTags.Ready),
                });

                endpoints.MapControllers();
            });
        }
    }
}

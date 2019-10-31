using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHSD.BuyingCatalogue.API.Extensions;
using NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;

namespace NHSD.BuyingCatalogue.API
{
    /// <summary>
    /// Represents a boostrapper for the application. Used as a starting point to configure the API.
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(AutoMapperProfile).Assembly)
                .AddCustomDbFactory()
                .AddCustomRepositories()
                .AddMediatR(typeof(ListSolutionsQuery).Assembly)
                .AddCustomHealthCheck()
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
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger()
                   .UseSwaggerUI(options =>
                  {
                      options.SwaggerEndpoint("/swagger/v1/swagger.json", "Buying Catalog API V1");
                  });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (healthCheckRegistration) => healthCheckRegistration.Tags.Contains(HealthCheckTags.Live)
                });

                endpoints.MapHealthChecks("/health/dependencies", new HealthCheckOptions
                {
                    Predicate = (healthCheckRegistration) => healthCheckRegistration.Tags.Contains(HealthCheckTags.Dependencies)
                });

                endpoints.MapControllers();
            });
        }
    }
}

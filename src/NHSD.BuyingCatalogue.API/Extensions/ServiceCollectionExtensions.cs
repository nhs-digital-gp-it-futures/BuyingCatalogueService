using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NHSD.BuyingCatalogue.API.Infrastructure.Filters;
using NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Capabilities.API;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.API;
using NHSD.BuyingCatalogue.Solutions.API;

namespace NHSD.BuyingCatalogue.API.Extensions
{
    /// <summary>
    /// Extends the functionality for the <see cref="IServiceCollection"/> class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the custom swagger settings for application.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <returns>The extended service collection instance.</returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Solutions API",
                    Version = "v1",
                    Description = "NHS Digital GP IT Buying Catalogue HTTP API"
                });
            });

            return services;
        }

        /// <summary>
        /// Adds the MVC controllers and custom settings.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <returns>The extended service collection instance.</returns>
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            Action<MvcOptions> op = options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add<SerilogLoggingActionFilter>();
            };

            Action<IMvcBuilder> controllerAction = builder => builder
                .AddNewtonsoftJson(jsonOptions =>
                {
                    jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    jsonOptions.SerializerSettings.Converters.Add(new TrimmingConverter());
                    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .RegisterCapabilityController(op, controllerAction)
                .RegisterSolutionListController(op, controllerAction)
                .RegisterSolutionController(op, controllerAction);

            return services;
        }

        /// <summary>
        /// Adds the health check middleware to provide feedback on the state of this application.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <param name="settings">The provider of application settings.</param>
        /// <returns>The extended service collection instance.</returns>
        public static IServiceCollection RegisterHealthChecks(this IServiceCollection services, ISettings settings)
        {
            services.AddHealthChecks()
                  .AddCheck(
                      name: "self",
                      check: () => HealthCheckResult.Healthy(),
                      tags: new[] { HealthCheckTags.Live })
                  .AddUrlGroup(
                      uri: new Uri($"{settings?.DocumentApiBaseUrl}/health/live"),
                      name: "DocumentAPI",
                      failureStatus: HealthStatus.Degraded,
                      tags: new[] { HealthCheckTags.Ready },
                      timeout: TimeSpan.FromSeconds(5))
                  .AddSqlServer(
                      connectionString: settings?.ConnectionString,
                      name: "db",
                      healthQuery: "SELECT 1;",
                      failureStatus: HealthStatus.Unhealthy,
                      tags: new[] { HealthCheckTags.Ready },
                      timeout: TimeSpan.FromSeconds(10));

            return services;
        }
    }
}

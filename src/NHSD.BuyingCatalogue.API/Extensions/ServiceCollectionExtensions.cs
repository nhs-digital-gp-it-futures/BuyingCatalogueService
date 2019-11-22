using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.Infrastructure.Filters;
using NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Capabilities.API;
using NHSD.BuyingCatalogue.SolutionList.API;

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
            };

            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .RegisterCapabilityController(op)
                .RegisterSolutionListController(op)
                .AddControllers(op)
                .AddNewtonsoftJson(jsonOptions =>
                {
                    jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			return services;
		}

        /// <summary>
        /// Adds the custom health check middleware to provide feedback on the state of this application.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <param name="configuration">The provider of application settings.</param>
        /// <returns>The extended service collection instance.</returns>
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { HealthCheckTags.Live })
                .AddCheck<PersistenceLayerHealthCheck>("persistence", tags: new [] { HealthCheckTags.Dependencies });

            return services;
        }
    }
}

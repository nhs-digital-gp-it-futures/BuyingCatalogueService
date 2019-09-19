using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.Infrastructure.Filters;
using NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Application.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Persistence.HealthChecks;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace NHSD.BuyingCatalogue.API.Extensions
{
    /// <summary>
    /// Extends the functionality for the <see cref="IServiceCollection"/> class.
    /// </summary>
    public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the project based database factory for the persistence layer.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomDbFactory(this IServiceCollection services)
		{
			services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

			return services;
		}

		/// <summary>
		/// Adds any application repositories.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomRepositories(this IServiceCollection services)
		{
			services.AddSingleton<ISolutionRepository, SolutionRepository>();
			services.AddSingleton<ICapabilityRepository, CapabilityRepository>();

			return services;
		}

		/// <summary>
		/// Adds the custom swagger settings for application.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();
				options.SwaggerDoc("v1", new Info
				{
					Title = "Solutions API",
					Version = "v1",
					Description = "NHS Digital GP IT Buying Catalogue HTTP API",
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
			services.AddMvc(options => 
			{
				options.Filters.Add(typeof(CustomExceptionFilter));
			})
			.AddJsonOptions((jsonOptions) => 
			{
				jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddControllersAsServices();

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
            services.AddSingleton<IRepositoryHealthCheck, RepositoryHealthCheck>();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { HealthCheckTags.Live })
                .AddCheck<PersistenceLayerHealthCheck>("persistence", tags: new [] { HealthCheckTags.Dependencies });

            return services;
        }
    }
}

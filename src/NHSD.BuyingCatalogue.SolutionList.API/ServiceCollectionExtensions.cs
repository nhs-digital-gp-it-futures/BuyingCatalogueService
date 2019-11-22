using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.SolutionList.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionListController(this IServiceCollection services, Action<MvcOptions> controllerOptions)
        {
            services
                .AddControllers(controllerOptions)
                .AddNewtonsoftJson(jsonOptions =>
                {
                    jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            return services;
        }
    }
}

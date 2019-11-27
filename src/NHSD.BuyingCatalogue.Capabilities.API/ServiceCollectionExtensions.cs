using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NHSD.BuyingCatalogue.Capabilities.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCapabilityController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
        {
            controllerAction(services.AddControllers(controllerOptions));
            return services;
        }
    }
}

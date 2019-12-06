using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Capabilities.API
{
    public static class ServiceCollectionExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "False Positive")]
        public static IServiceCollection RegisterCapabilityController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
        {
            controllerAction.ThrowIfNull();

            controllerAction(services.AddControllers(controllerOptions));
            return services;
        }
    }
}

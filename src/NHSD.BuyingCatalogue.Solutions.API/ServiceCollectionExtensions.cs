using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
        {
            if (controllerAction is null)
            {
                throw new ArgumentNullException(nameof(controllerAction));
            }

            controllerAction.Invoke(services.AddControllers(controllerOptions));
            return services;
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NHSD.BuyingCatalogue.Solution.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
        {
            controllerAction(services.AddControllers(controllerOptions));
            return services;
        }
    }
}

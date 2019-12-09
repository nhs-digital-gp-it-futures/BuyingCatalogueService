using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
        {
            controllerAction.ThrowIfNull().Invoke(services.AddControllers(controllerOptions));
            return services;
        }
    }
}

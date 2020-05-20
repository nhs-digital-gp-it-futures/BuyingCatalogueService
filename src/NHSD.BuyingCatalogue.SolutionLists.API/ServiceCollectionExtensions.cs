using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NHSD.BuyingCatalogue.SolutionLists.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionListController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
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

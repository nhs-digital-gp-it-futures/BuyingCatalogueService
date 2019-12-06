using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.SolutionLists.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionListController(this IServiceCollection services, Action<MvcOptions> controllerOptions, Action<IMvcBuilder> controllerAction)
        {
            controllerAction.ThrowIfNull().Invoke(services.AddControllers(controllerOptions));
            return services;
        }
    }
}

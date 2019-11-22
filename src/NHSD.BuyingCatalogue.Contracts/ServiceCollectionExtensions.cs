using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace NHSD.BuyingCatalogue.Contracts
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRequests(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}

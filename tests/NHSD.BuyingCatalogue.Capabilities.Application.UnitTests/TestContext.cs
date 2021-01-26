using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Application.Mapping;
using NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Capabilities.Contracts;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Application.UnitTests
{
    internal sealed class TestContext
    {
        private readonly Scope scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            var myAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(CapabilityAutoMapperProfile)),
            };

            serviceCollection
                .AddAutoMapper(myAssemblies)
                .AddMediatR(myAssemblies);

            serviceCollection.RegisterCapabilitiesApplication();

            serviceCollection.AddSingleton<Scope>();
            scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        public Mock<ICapabilityRepository> MockCapabilityRepository { get; private set; }

        public ListCapabilitiesHandler ListCapabilitiesHandler => (ListCapabilitiesHandler)scope.ListCapabilitiesHandler;

        private void RegisterDependencies(IServiceCollection serviceCollection)
        {
            MockCapabilityRepository = new Mock<ICapabilityRepository>();
            serviceCollection.AddSingleton(MockCapabilityRepository.Object);
        }

        private sealed class Scope
        {
            public Scope(IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> listCapabilitiesHandler)
            {
                ListCapabilitiesHandler = listCapabilitiesHandler;
            }

            public IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> ListCapabilitiesHandler { get; }
        }
    }
}

using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Application.Mapping;
using NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Contracts.Capability;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ICapabilityRepository> MockCapabilityRepository { get; private set; }

        public ListCapabilitiesHandler ListCapabilitiesHandler => (ListCapabilitiesHandler)_scope.ListCapabilitiesHandler;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            var myAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(CapabilityAutoMapperProfile)),
            };
            serviceCollection.AddAutoMapper(myAssemblies);
            serviceCollection.RegisterCapabilitiesApplication();

            serviceCollection.AddSingleton<Scope>();
            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
            MockCapabilityRepository = new Mock<ICapabilityRepository>();
            serviceCollection.AddSingleton<ICapabilityRepository>(MockCapabilityRepository.Object);
        }

        private class Scope
        {
            public IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> ListCapabilitiesHandler { get; }

            public Scope(IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> listCapabilitiesHandler)
            {
                ListCapabilitiesHandler = listCapabilitiesHandler;
            }
        }
    }
}

using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.SolutionLists.Application.Mapping;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ISolutionListRepository> MockSolutionListRepository { get; private set; }

        public ListSolutionsHandler ListSolutionsHandler => (ListSolutionsHandler)_scope.ListSolutionsHandler;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            var myAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(SolutionListAutoMapperProfile))
            };
            _scope = serviceCollection
                .AddAutoMapper(myAssemblies)
                .AddMediatR(myAssemblies)
                .RegisterSolutionListApplication()
                .AddSingleton<Scope>()
                .BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
            MockSolutionListRepository = new Mock<ISolutionListRepository>();
            serviceCollection.AddSingleton<ISolutionListRepository>(MockSolutionListRepository.Object);
        }

        private class Scope
        {
            public IRequestHandler<ListSolutionsQuery, ISolutionList> ListSolutionsHandler { get; }

            public Scope(IRequestHandler<ListSolutionsQuery, ISolutionList> listSolutionsHandler)
            {
                ListSolutionsHandler = listSolutionsHandler;
            }
        }
    }
}

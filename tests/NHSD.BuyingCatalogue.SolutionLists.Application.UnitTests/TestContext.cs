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
    internal sealed class TestContext
    {
        private readonly Scope scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            var myAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(SolutionListAutoMapperProfile)),
            };

            scope = serviceCollection
                .AddAutoMapper(myAssemblies)
                .AddMediatR(myAssemblies)
                .RegisterSolutionListApplication()
                .AddSingleton<Scope>()
                .BuildServiceProvider()
                .GetService<Scope>();
        }

        public Mock<ISolutionListRepository> MockSolutionListRepository { get; private set; }

        public ListSolutionsHandler ListSolutionsHandler => (ListSolutionsHandler)scope.ListSolutionsHandler;

        private void RegisterDependencies(IServiceCollection serviceCollection)
        {
            MockSolutionListRepository = new Mock<ISolutionListRepository>();
            serviceCollection.AddSingleton(MockSolutionListRepository.Object);
        }

        private sealed class Scope
        {
            public Scope(IRequestHandler<ListSolutionsQuery, ISolutionList> listSolutionsHandler)
            {
                ListSolutionsHandler = listSolutionsHandler;
            }

            public IRequestHandler<ListSolutionsQuery, ISolutionList> ListSolutionsHandler { get; }
        }
    }
}

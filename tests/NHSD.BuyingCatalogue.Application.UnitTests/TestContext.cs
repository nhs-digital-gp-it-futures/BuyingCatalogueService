using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ICapabilityRepository> MockCapabilityRepository { get; private set; }

        public Mock<ISolutionRepository> MockSolutionRepository { get; private set; }

        public Mock<IMarketingDetailRepository> MockMarketingDetailRepository { get; private set; }

        public ListCapabilitiesHandler ListCapabilitiesHandler => (ListCapabilitiesHandler)_scope.ListCapabilitiesHandler;

        public ListSolutionsHandler ListSolutionsHandler => (ListSolutionsHandler)_scope.ListSolutionsHandler;

        public GetSolutionByIdHandler GetSolutionByIdHandler => (GetSolutionByIdHandler)_scope.GetSolutionByIdHandler;

        public UpdateSolutionHandler UpdateSolutionHandler => (UpdateSolutionHandler)_scope.UpdateSolutionHandler;

        public UpdateSolutionSummaryHandler UpdateSolutionSummaryHandler => (UpdateSolutionSummaryHandler)_scope.UpdateSolutionSummaryHandler;

        public UpdateSolutionFeaturesHandler UpdateSolutionFeaturesHandler => (UpdateSolutionFeaturesHandler)_scope.UpdateSolutionFeaturesHandler;

        public SubmitSolutionForReviewHandler SubmitSolutionForReviewHandler => (SubmitSolutionForReviewHandler)_scope.SubmitSolutionForReviewHandler;

        private Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            serviceCollection.AddSingleton<IMapper>(GetAutoMapper());
            serviceCollection.RegisterApplication();

            serviceCollection.AddTransient<IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesResult>, ListCapabilitiesHandler>();
            serviceCollection.AddTransient<IRequestHandler<ListSolutionsQuery, ListSolutionsResult>, ListSolutionsHandler>();
            serviceCollection.AddTransient<IRequestHandler<GetSolutionByIdQuery, GetSolutionByIdResult>, GetSolutionByIdHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionCommand>, UpdateSolutionHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionSummaryCommand>, UpdateSolutionSummaryHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionFeaturesCommand>, UpdateSolutionFeaturesHandler>();
            serviceCollection.AddTransient<IRequestHandler<SubmitSolutionForReviewCommand>, SubmitSolutionForReviewHandler>();

            serviceCollection.AddSingleton<Scope>();
            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
            MockCapabilityRepository = new Mock<ICapabilityRepository>();
            serviceCollection.AddSingleton<ICapabilityRepository>(MockCapabilityRepository.Object);
            MockSolutionRepository = new Mock<ISolutionRepository>();
            serviceCollection.AddSingleton<ISolutionRepository>(MockSolutionRepository.Object);
            MockMarketingDetailRepository = new Mock<IMarketingDetailRepository>();
            serviceCollection.AddSingleton<IMarketingDetailRepository>(MockMarketingDetailRepository.Object);
        }

        private IMapper GetAutoMapper()
        {
            var config = new MapperConfiguration(opts => opts.AddProfile(new AutoMapperProfile()));
            return config.CreateMapper();
        }

        private class Scope
        {
            public IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesResult> ListCapabilitiesHandler { get; }

            public IRequestHandler<ListSolutionsQuery, ListSolutionsResult> ListSolutionsHandler { get; }

            public IRequestHandler<UpdateSolutionCommand> UpdateSolutionHandler { get; }

            public IRequestHandler<GetSolutionByIdQuery, GetSolutionByIdResult> GetSolutionByIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand> UpdateSolutionFeaturesHandler { get; }

            public Scope(IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesResult> listCapabilitiesHandler,
                IRequestHandler<ListSolutionsQuery, ListSolutionsResult> listSolutionsHandler,
                IRequestHandler<UpdateSolutionCommand> updateSolutionHandler,
                IRequestHandler<GetSolutionByIdQuery, GetSolutionByIdResult> getSolutionByIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand> updateSolutionFeaturesHandler)
            {
                ListCapabilitiesHandler = listCapabilitiesHandler;
                ListSolutionsHandler = listSolutionsHandler;
                UpdateSolutionHandler = updateSolutionHandler;
                GetSolutionByIdHandler = getSolutionByIdHandler;
                SubmitSolutionForReviewHandler = submitSolutionForReviewHandler;
                UpdateSolutionSummaryHandler = updateSolutionSummaryHandler;
                UpdateSolutionFeaturesHandler = updateSolutionFeaturesHandler;
            }
        }
    }
}

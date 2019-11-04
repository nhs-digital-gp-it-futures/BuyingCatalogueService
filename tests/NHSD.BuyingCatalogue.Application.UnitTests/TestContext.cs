using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;

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

        public UpdateSolutionSummaryHandler UpdateSolutionSummaryHandler => (UpdateSolutionSummaryHandler)_scope.UpdateSolutionSummaryHandler;

        public UpdateSolutionFeaturesHandler UpdateSolutionFeaturesHandler => (UpdateSolutionFeaturesHandler)_scope.UpdateSolutionFeaturesHandler;

        public SubmitSolutionForReviewHandler SubmitSolutionForReviewHandler => (SubmitSolutionForReviewHandler)_scope.SubmitSolutionForReviewHandler;

        public UpdateSolutionClientApplicationTypesHandler UpdateSolutionClientApplicationTypesHandler => (UpdateSolutionClientApplicationTypesHandler)_scope.UpdateSolutionClientApplicationTypesHandler;

        public UpdateSolutionBrowsersSupportedHandler UpdateSolutionBrowsersSupportedHandler => (UpdateSolutionBrowsersSupportedHandler)_scope.UpdateSolutionBrowsersSupportedHandler;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            serviceCollection.AddSingleton<IMapper>(GetAutoMapper());
            serviceCollection.RegisterApplication();

            serviceCollection.AddTransient<IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesResult>, ListCapabilitiesHandler>();
            serviceCollection.AddTransient<IRequestHandler<ListSolutionsQuery, ListSolutionsResult>, ListSolutionsHandler>();
            serviceCollection.AddTransient<IRequestHandler<GetSolutionByIdQuery, Solution>, GetSolutionByIdHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult>, UpdateSolutionSummaryHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidatorResult>, UpdateSolutionFeaturesHandler>();
            serviceCollection.AddTransient<IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewResult>, SubmitSolutionForReviewHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionClientApplicationTypesCommand>, UpdateSolutionClientApplicationTypesHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionBrowsersSupportedCommand>, UpdateSolutionBrowsersSupportedHandler>();

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

            public IRequestHandler<GetSolutionByIdQuery, Solution> GetSolutionByIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewResult> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidatorResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand> UpdateSolutionClientApplicationTypesHandler { get; }


            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand> UpdateSolutionBrowsersSupportedHandler { get; }

            public Scope(IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesResult> listCapabilitiesHandler,
                IRequestHandler<ListSolutionsQuery, ListSolutionsResult> listSolutionsHandler,
                IRequestHandler<GetSolutionByIdQuery, Solution> getSolutionByIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewResult> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidatorResult> updateSolutionFeaturesHandler,
                IRequestHandler<UpdateSolutionClientApplicationTypesCommand> updateSolutionClientApplicationTypesHandler,
                IRequestHandler<UpdateSolutionBrowsersSupportedCommand> updateSolutionBrowsersSupportedHandler)
            {
                ListCapabilitiesHandler = listCapabilitiesHandler;
                ListSolutionsHandler = listSolutionsHandler;
                GetSolutionByIdHandler = getSolutionByIdHandler;
                SubmitSolutionForReviewHandler = submitSolutionForReviewHandler;
                UpdateSolutionSummaryHandler = updateSolutionSummaryHandler;
                UpdateSolutionFeaturesHandler = updateSolutionFeaturesHandler;
                UpdateSolutionClientApplicationTypesHandler = updateSolutionClientApplicationTypesHandler;
                UpdateSolutionBrowsersSupportedHandler = updateSolutionBrowsersSupportedHandler;
            }
        }
    }
}

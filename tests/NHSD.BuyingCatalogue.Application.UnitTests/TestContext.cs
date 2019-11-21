using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Capability;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Contracts.SolutionList;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ICapabilityRepository> MockCapabilityRepository { get; private set; }

        public Mock<ISolutionRepository> MockSolutionRepository { get; private set; }

        public Mock<ISolutionListRepository> MockSolutionListRepository { get; private set; }

        public Mock<ISolutionDetailRepository> MockSolutionDetailRepository { get; private set; }

        public Mock<ISolutionCapabilityRepository> MockSolutionCapabilityRepository { get; private set; }

        public Mock<IMarketingContactRepository> MockMarketingContactRepository { get; private set; }

        public ListCapabilitiesHandler ListCapabilitiesHandler => (ListCapabilitiesHandler)_scope.ListCapabilitiesHandler;

        public ListSolutionsHandler ListSolutionsHandler => (ListSolutionsHandler)_scope.ListSolutionsHandler;

        public GetSolutionByIdHandler GetSolutionByIdHandler => (GetSolutionByIdHandler)_scope.GetSolutionByIdHandler;

        public GetClientApplicationBySolutionIdHandler GetClientApplicationBySolutionIdHandler => (GetClientApplicationBySolutionIdHandler)_scope.GetClientApplicationBySolutionIdHandler;

        public UpdateSolutionSummaryHandler UpdateSolutionSummaryHandler => (UpdateSolutionSummaryHandler)_scope.UpdateSolutionSummaryHandler;

        public UpdateSolutionFeaturesHandler UpdateSolutionFeaturesHandler => (UpdateSolutionFeaturesHandler)_scope.UpdateSolutionFeaturesHandler;

        public SubmitSolutionForReviewHandler SubmitSolutionForReviewHandler => (SubmitSolutionForReviewHandler)_scope.SubmitSolutionForReviewHandler;

        public UpdateSolutionClientApplicationTypesHandler UpdateSolutionClientApplicationTypesHandler => (UpdateSolutionClientApplicationTypesHandler)_scope.UpdateSolutionClientApplicationTypesHandler;

        public UpdateSolutionBrowsersSupportedHandler UpdateSolutionBrowsersSupportedHandler => (UpdateSolutionBrowsersSupportedHandler)_scope.UpdateSolutionBrowsersSupportedHandler;

        public UpdateSolutionPluginsHandler UpdateSolutionPluginsHandler =>
            (UpdateSolutionPluginsHandler)_scope.UpdateSolutionPluginsHandler;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            serviceCollection.RegisterApplication();

            serviceCollection.AddTransient<IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>>, ListCapabilitiesHandler>();
            serviceCollection.AddTransient<IRequestHandler<ListSolutionsQuery, ISolutionList>, ListSolutionsHandler>();
            serviceCollection.AddTransient<IRequestHandler<GetSolutionByIdQuery, ISolution>, GetSolutionByIdHandler>();
            serviceCollection.AddTransient<IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication>, GetClientApplicationBySolutionIdHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult>, UpdateSolutionSummaryHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult>, UpdateSolutionFeaturesHandler>();
            serviceCollection.AddTransient<IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult>, SubmitSolutionForReviewHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult>, UpdateSolutionClientApplicationTypesHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult>, UpdateSolutionBrowsersSupportedHandler>();
            serviceCollection
                .AddTransient<IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult>,
                    UpdateSolutionPluginsHandler>();

            serviceCollection.AddSingleton<Scope>();
            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
            MockCapabilityRepository = new Mock<ICapabilityRepository>();
            serviceCollection.AddSingleton<ICapabilityRepository>(MockCapabilityRepository.Object);
            MockSolutionRepository = new Mock<ISolutionRepository>();
            serviceCollection.AddSingleton<ISolutionRepository>(MockSolutionRepository.Object);
            MockSolutionListRepository = new Mock<ISolutionListRepository>();
            serviceCollection.AddSingleton<ISolutionListRepository>(MockSolutionListRepository.Object);
            MockSolutionDetailRepository = new Mock<ISolutionDetailRepository>();
            serviceCollection.AddSingleton<ISolutionDetailRepository>(MockSolutionDetailRepository.Object);
            MockSolutionCapabilityRepository = new Mock<ISolutionCapabilityRepository>();
            serviceCollection.AddSingleton<ISolutionCapabilityRepository>(MockSolutionCapabilityRepository.Object);
            MockMarketingContactRepository = new Mock<IMarketingContactRepository>();
            serviceCollection.AddSingleton<IMarketingContactRepository>(MockMarketingContactRepository.Object);
        }

        private class Scope
        {
            public IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> ListCapabilitiesHandler { get; }

            public IRequestHandler<ListSolutionsQuery, ISolutionList> ListSolutionsHandler { get; }

            public IRequestHandler<GetSolutionByIdQuery, ISolution> GetSolutionByIdHandler { get; }

            public IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> GetClientApplicationBySolutionIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult> UpdateSolutionClientApplicationTypesHandler { get; }

            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult> UpdateSolutionBrowsersSupportedHandler { get; }

            public IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult> UpdateSolutionPluginsHandler { get; }

            public Scope(IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> listCapabilitiesHandler,
                IRequestHandler<ListSolutionsQuery, ISolutionList> listSolutionsHandler,
                IRequestHandler<GetSolutionByIdQuery, ISolution> getSolutionByIdHandler,
                IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> getClientApplicationBySolutionIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult> updateSolutionFeaturesHandler,
                IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult> updateSolutionClientApplicationTypesHandler,
                IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult> updateSolutionBrowsersSupportedHandler,
                IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult> updateSolutionPluginsHandler)
            {
                ListCapabilitiesHandler = listCapabilitiesHandler;
                ListSolutionsHandler = listSolutionsHandler;
                GetSolutionByIdHandler = getSolutionByIdHandler;
                GetClientApplicationBySolutionIdHandler = getClientApplicationBySolutionIdHandler;
                SubmitSolutionForReviewHandler = submitSolutionForReviewHandler;
                UpdateSolutionSummaryHandler = updateSolutionSummaryHandler;
                UpdateSolutionFeaturesHandler = updateSolutionFeaturesHandler;
                UpdateSolutionClientApplicationTypesHandler = updateSolutionClientApplicationTypesHandler;
                UpdateSolutionBrowsersSupportedHandler = updateSolutionBrowsersSupportedHandler;
                UpdateSolutionPluginsHandler = updateSolutionPluginsHandler;
            }
        }
    }
}

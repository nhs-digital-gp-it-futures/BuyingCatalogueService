using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Contracts.SolutionList;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.SolutionLists.Application;
using NHSD.BuyingCatalogue.SolutionLists.Application.Mapping;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;


namespace NHSD.BuyingCatalogue.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ISolutionRepository> MockSolutionRepository { get; private set; }

        public Mock<ISolutionListRepository> MockSolutionListRepository { get; private set; }

        public Mock<ISolutionDetailRepository> MockSolutionDetailRepository { get; private set; }

        public Mock<ISolutionCapabilityRepository> MockSolutionCapabilityRepository { get; private set; }

        public Mock<IMarketingContactRepository> MockMarketingContactRepository { get; private set; }

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

            var myAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(SolutionAutoMapperProfile)),
                Assembly.GetAssembly(typeof(SolutionListAutoMapperProfile)),
                Assembly.GetAssembly(typeof(SolutionAutoMapperProfile))
            };
            _scope = serviceCollection
                .AddAutoMapper(myAssemblies)
                .AddMediatR(myAssemblies)
                .RegisterApplication()
                .RegisterSolutionListApplication()
                .AddSingleton<Scope>()
                .BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
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
            public IRequestHandler<ListSolutionsQuery, ISolutionList> ListSolutionsHandler { get; }

            public IRequestHandler<GetSolutionByIdQuery, ISolution> GetSolutionByIdHandler { get; }

            public IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> GetClientApplicationBySolutionIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult> UpdateSolutionClientApplicationTypesHandler { get; }

            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult> UpdateSolutionBrowsersSupportedHandler { get; }

            public IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult> UpdateSolutionPluginsHandler { get; }

            public Scope(IRequestHandler<ListSolutionsQuery, ISolutionList> listSolutionsHandler,
                IRequestHandler<GetSolutionByIdQuery, ISolution> getSolutionByIdHandler,
                IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> getClientApplicationBySolutionIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult> updateSolutionFeaturesHandler,
                IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult> updateSolutionClientApplicationTypesHandler,
                IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult> updateSolutionBrowsersSupportedHandler,
                IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult> updateSolutionPluginsHandler)
            {
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

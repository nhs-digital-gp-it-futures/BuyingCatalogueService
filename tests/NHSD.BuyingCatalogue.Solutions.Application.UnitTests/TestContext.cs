using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Mapping;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ISolutionRepository> MockSolutionRepository { get; private set; }

        public Mock<ISolutionDetailRepository> MockSolutionDetailRepository { get; private set; }

        public Mock<ISolutionCapabilityRepository> MockSolutionCapabilityRepository { get; private set; }

        public Mock<IMarketingContactRepository> MockMarketingContactRepository { get; private set; }

        public GetSolutionByIdHandler GetSolutionByIdHandler => (GetSolutionByIdHandler)_scope.GetSolutionByIdHandler;

        public GetClientApplicationBySolutionIdHandler GetClientApplicationBySolutionIdHandler => (GetClientApplicationBySolutionIdHandler)_scope.GetClientApplicationBySolutionIdHandler;

        public UpdateSolutionSummaryHandler UpdateSolutionSummaryHandler => (UpdateSolutionSummaryHandler)_scope.UpdateSolutionSummaryHandler;

        public UpdateSolutionFeaturesHandler UpdateSolutionFeaturesHandler => (UpdateSolutionFeaturesHandler)_scope.UpdateSolutionFeaturesHandler;

        public SubmitSolutionForReviewHandler SubmitSolutionForReviewHandler => (SubmitSolutionForReviewHandler)_scope.SubmitSolutionForReviewHandler;

        public UpdateSolutionClientApplicationTypesHandler UpdateSolutionClientApplicationTypesHandler => (UpdateSolutionClientApplicationTypesHandler)_scope.UpdateSolutionClientApplicationTypesHandler;

        public UpdateSolutionBrowsersSupportedHandler UpdateSolutionBrowsersSupportedHandler => (UpdateSolutionBrowsersSupportedHandler)_scope.UpdateSolutionBrowsersSupportedHandler;
        public UpdateSolutionContactDetailsHandler UpdateSolutionContactDetailsHandler => (UpdateSolutionContactDetailsHandler)_scope.UpdateSolutionContactDetailsHandler;

        public GetContactDetailBySolutionIdHandler GetContactDetailBySolutionIdHandler =>
            (GetContactDetailBySolutionIdHandler)_scope.GetContactDetailBySolutionIdHandler;

        public UpdateSolutionPluginsHandler UpdateSolutionPluginsHandler =>
            (UpdateSolutionPluginsHandler)_scope.UpdateSolutionPluginsHandler;

        public UpdateSolutionBrowserHardwareRequirementsHandler UpdateSolutionBrowserHardwareRequirementsHandler =>
            (UpdateSolutionBrowserHardwareRequirementsHandler)_scope.UpdateSolutionBrowserHardwareRequirementsHandler;

        public UpdateSolutionConnectivityAndResolutionHandler UpdateSolutionConnectivityAndResolutionHandler =>
            (UpdateSolutionConnectivityAndResolutionHandler)_scope.UpdateSolutionConnectivityAndResolutionHandler;
        public UpdateSolutionBrowserAdditionalInformationHandler UpdateSolutionBrowserAdditionalInformationHandler =>
            (UpdateSolutionBrowserAdditionalInformationHandler)_scope.UpdateSolutionBrowserAdditionalInformationHandler;

        public UpdateSolutionBrowserMobileFirstHandler UpdateSolutionBrowserMobileFirstHandler =>
            (UpdateSolutionBrowserMobileFirstHandler)_scope.UpdateSolutionBrowserMobileFirstHandler;

        public UpdateSolutionMobileOperatingSystemsHandler UpdateSolutionMobileOperatingSystemsHandler =>
            (UpdateSolutionMobileOperatingSystemsHandler)_scope.UpdateSolutionMobileOperatingSystemsHandler;

        public UpdateSolutionMobileConnectionDetailsHandler UpdateSolutionMobileConnectionDetailsHandler =>
            (UpdateSolutionMobileConnectionDetailsHandler)_scope.UpdateSolutionMobileConnectionDetailsHandler;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            var myAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(SolutionAutoMapperProfile))
            };
            _scope = serviceCollection
                .AddAutoMapper(myAssemblies)
                .AddMediatR(myAssemblies)
                .RegisterSolutionApplication()
                .AddSingleton<Scope>()
                .BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
            MockSolutionRepository = new Mock<ISolutionRepository>();
            serviceCollection.AddSingleton<ISolutionRepository>(MockSolutionRepository.Object);
            MockSolutionDetailRepository = new Mock<ISolutionDetailRepository>();
            serviceCollection.AddSingleton<ISolutionDetailRepository>(MockSolutionDetailRepository.Object);
            MockSolutionCapabilityRepository = new Mock<ISolutionCapabilityRepository>();
            serviceCollection.AddSingleton<ISolutionCapabilityRepository>(MockSolutionCapabilityRepository.Object);
            MockMarketingContactRepository = new Mock<IMarketingContactRepository>();
            serviceCollection.AddSingleton<IMarketingContactRepository>(MockMarketingContactRepository.Object);
        }

        private class Scope
        {
            public IRequestHandler<GetSolutionByIdQuery, ISolution> GetSolutionByIdHandler { get; }

            public IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> GetClientApplicationBySolutionIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand, RequiredMaxLengthResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, MaxLengthResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand, RequiredResult> UpdateSolutionClientApplicationTypesHandler { get; }

            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand, RequiredResult> UpdateSolutionBrowsersSupportedHandler { get; }

            public IRequestHandler<UpdateSolutionContactDetailsCommand, MaxLengthResult> UpdateSolutionContactDetailsHandler { get; }

            public IRequestHandler<UpdateSolutionPluginsCommand, RequiredMaxLengthResult> UpdateSolutionPluginsHandler { get; }

            public IRequestHandler<GetContactDetailBySolutionIdQuery, IEnumerable<IContact>> GetContactDetailBySolutionIdHandler { get; }

            public IRequestHandler<UpdateSolutionBrowserHardwareRequirementsCommand, MaxLengthResult> UpdateSolutionBrowserHardwareRequirementsHandler { get; }

            public IRequestHandler<UpdateSolutionConnectivityAndResolutionCommand, RequiredResult> UpdateSolutionConnectivityAndResolutionHandler { get; }
            public IRequestHandler<UpdateSolutionBrowserAdditionalInformationCommand, MaxLengthResult> UpdateSolutionBrowserAdditionalInformationHandler { get; }

            public IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, RequiredResult> UpdateSolutionBrowserMobileFirstHandler { get; }

            public IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, RequiredMaxLengthResult> UpdateSolutionMobileOperatingSystemsHandler {get;}

            public IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult> UpdateSolutionMobileConnectionDetailsHandler {get;}

            public Scope(IRequestHandler<GetSolutionByIdQuery, ISolution> getSolutionByIdHandler,
                IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> getClientApplicationBySolutionIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand, RequiredMaxLengthResult> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand, MaxLengthResult> updateSolutionFeaturesHandler,
                IRequestHandler<UpdateSolutionClientApplicationTypesCommand, RequiredResult> updateSolutionClientApplicationTypesHandler,
                IRequestHandler<UpdateSolutionBrowsersSupportedCommand, RequiredResult> updateSolutionBrowsersSupportedHandler,
                IRequestHandler<UpdateSolutionPluginsCommand, RequiredMaxLengthResult> updateSolutionPluginsHandler,
                IRequestHandler<UpdateSolutionContactDetailsCommand, MaxLengthResult> updateSolutionContactDetailsHandler,
                IRequestHandler<GetContactDetailBySolutionIdQuery, IEnumerable<IContact>> getContactDetailBySolutionIdHandler,
                IRequestHandler<UpdateSolutionBrowserHardwareRequirementsCommand, MaxLengthResult> updateSolutionBrowserHardwareRequirementsHandler,
                IRequestHandler<UpdateSolutionConnectivityAndResolutionCommand, RequiredResult> updateSolutionConnectivityAndResolutionHandler,
                IRequestHandler<UpdateSolutionBrowserAdditionalInformationCommand, MaxLengthResult> updateSolutionBrowserAdditionalInformationHandler,
                IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, RequiredResult> updateSolutionBrowserMobileFirstHandler,
                IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, RequiredMaxLengthResult> updateSolutionMobileOperatingSystemsHandler,
                IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult> updateSolutionMobileConnectionDetailsHandler)
            {
                GetSolutionByIdHandler = getSolutionByIdHandler;
                GetClientApplicationBySolutionIdHandler = getClientApplicationBySolutionIdHandler;
                SubmitSolutionForReviewHandler = submitSolutionForReviewHandler;
                UpdateSolutionSummaryHandler = updateSolutionSummaryHandler;
                UpdateSolutionFeaturesHandler = updateSolutionFeaturesHandler;
                UpdateSolutionClientApplicationTypesHandler = updateSolutionClientApplicationTypesHandler;
                UpdateSolutionBrowsersSupportedHandler = updateSolutionBrowsersSupportedHandler;
                UpdateSolutionPluginsHandler = updateSolutionPluginsHandler;
                UpdateSolutionContactDetailsHandler = updateSolutionContactDetailsHandler;
                GetContactDetailBySolutionIdHandler = getContactDetailBySolutionIdHandler;
                UpdateSolutionBrowserHardwareRequirementsHandler = updateSolutionBrowserHardwareRequirementsHandler;
                UpdateSolutionBrowserAdditionalInformationHandler = updateSolutionBrowserAdditionalInformationHandler;
                UpdateSolutionConnectivityAndResolutionHandler = updateSolutionConnectivityAndResolutionHandler;
                UpdateSolutionBrowserMobileFirstHandler = updateSolutionBrowserMobileFirstHandler;
                UpdateSolutionMobileOperatingSystemsHandler = updateSolutionMobileOperatingSystemsHandler;
                UpdateSolutionMobileConnectionDetailsHandler = updateSolutionMobileConnectionDetailsHandler;
            }
        }
    }
}

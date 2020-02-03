using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.HybridHostingType;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Mapping;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ISolutionRepository> MockSolutionRepository { get; private set; }

        public Mock<ISolutionDetailRepository> MockSolutionDetailRepository { get; private set; }

        public Mock<ISolutionCapabilityRepository> MockSolutionCapabilityRepository { get; private set; }

        public Mock<IMarketingContactRepository> MockMarketingContactRepository { get; private set; }

        public Mock<ISupplierRepository> MockSupplierRepository { get; private set; }

        public Mock<IDocumentRepository> MockDocumentRepository { get; private set; }

        public GetSolutionByIdHandler GetSolutionByIdHandler => (GetSolutionByIdHandler)_scope.GetSolutionByIdHandler;

        public GetRoadMapBySolutionIdHandler GetRoadMapBySolutionIdHandler => (GetRoadMapBySolutionIdHandler)_scope.GetRoadMapBySolutionIdHandler;

        public GetIntegrationsBySolutionIdHandler GetIntegrationsBySolutionIdHandler => (GetIntegrationsBySolutionIdHandler)_scope.GetIntegrationsBySolutionIdHandler;

        public GetClientApplicationBySolutionIdHandler GetClientApplicationBySolutionIdHandler => (GetClientApplicationBySolutionIdHandler)_scope.GetClientApplicationBySolutionIdHandler;

        public GetHostingBySolutionIdHandler GetHostingBySolutionIdHandler => (GetHostingBySolutionIdHandler)_scope.GetHostingBySolutionIdHandler;

        public GetSupplierBySolutionIdHandler GetSupplierBySolutionIdHandler => (GetSupplierBySolutionIdHandler)_scope.GetSupplierBySolutionIdHandler;

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
        public UpdateBrowserBasedAdditionalInformationHandler UpdateSolutionBrowserAdditionalInformationHandler =>
            (UpdateBrowserBasedAdditionalInformationHandler)_scope.UpdateSolutionBrowserAdditionalInformationHandler;

        public UpdateSolutionBrowserMobileFirstHandler UpdateSolutionBrowserMobileFirstHandler =>
            (UpdateSolutionBrowserMobileFirstHandler)_scope.UpdateSolutionBrowserMobileFirstHandler;

        public UpdateSolutionMobileOperatingSystemsHandler UpdateSolutionMobileOperatingSystemsHandler =>
            (UpdateSolutionMobileOperatingSystemsHandler)_scope.UpdateSolutionMobileOperatingSystemsHandler;

        public UpdateSolutionMobileConnectionDetailsHandler UpdateSolutionMobileConnectionDetailsHandler =>
            (UpdateSolutionMobileConnectionDetailsHandler)_scope.UpdateSolutionMobileConnectionDetailsHandler;

        public UpdateSolutionNativeMobileFirstHandler UpdateSolutionNativeMobileFirstHandler =>
            (UpdateSolutionNativeMobileFirstHandler)_scope.UpdateSolutionNativeMobileFirstHandler;

        public UpdateSolutionMobileMemoryStorageHandler UpdateSolutionMobileMemoryStorageHandler =>
            (UpdateSolutionMobileMemoryStorageHandler)_scope.UpdateSolutionMobileMemoryStorageHandler;

        public UpdateSolutionMobileThirdPartyHandler UpdateSolutionMobileThirdPartyHandler =>
            (UpdateSolutionMobileThirdPartyHandler)_scope.UpdateSolutionMobileThirdHandler;

        public UpdateSolutionNativeMobileHardwareRequirementsHandler UpdateSolutionNativeMobileHardwareRequirementsHandler =>
            (UpdateSolutionNativeMobileHardwareRequirementsHandler)_scope.UpdateSolutionNativeMobileHardwareRequirementsHandler;
        public UpdateSolutionNativeMobileAdditionalInformationHandler UpdateSolutionNativeMobileAdditionalInformationHandler =>
        (UpdateSolutionNativeMobileAdditionalInformationHandler)_scope.UpdateSolutionNativeMobileAdditionalInformationHandler;

        public UpdateNativeDesktopHardwareRequirementsHandler UpdateNativeDesktopHardwareRequirementsHandler =>
            (UpdateNativeDesktopHardwareRequirementsHandler)_scope.UpdateNativeDesktopHardwareRequirementsHandler;
        public UpdateSolutionNativeDesktopOperatingSystemsHandler UpdateSolutionNativeDesktopOperatingSystemsHandler =>
            (UpdateSolutionNativeDesktopOperatingSystemsHandler)_scope.UpdateSolutionNativeDesktopOperatingSystemsHandler;

        public UpdateSolutionNativeDesktopThirdPartyHandler UpdateSolutionNativeDesktopThirdPartyHandler =>
            (UpdateSolutionNativeDesktopThirdPartyHandler)_scope.UpdateSolutionNativeDesktopThirdPartyHandler;

        public UpdateSolutionNativeDesktopConnectivityDetailsHandler
            UpdateSolutionNativeDesktopConnectivityDetailsHandler =>
            (UpdateSolutionNativeDesktopConnectivityDetailsHandler)_scope
                .UpdateSolutionNativeDesktopConnectivityDetailsHandler;

        public UpdateNativeDesktopMemoryAndStorageHandler UpdateNativeDesktopMemoryAndStorageHandler =>
            (UpdateNativeDesktopMemoryAndStorageHandler)_scope.UpdateNativeDesktopMemoryAndStorageHandler;

        public UpdateNativeDesktopAdditionalInformationHandler UpdateNativeDesktopAdditionalInformationHandler =>
            (UpdateNativeDesktopAdditionalInformationHandler)_scope.UpdateNativeDesktopAdditionalInformationHandler;

        public UpdatePublicCloudHandler UpdatePublicCloudHandler =>
            (UpdatePublicCloudHandler)_scope.UpdatePublicCloudHandler;

        public UpdatePrivateCloudHandler UpdatePrivateCloudHandler =>
            (UpdatePrivateCloudHandler)_scope.UpdatePrivateCloudHandler;

        public UpdateOnPremiseHandler UpdateOnPremiseHandler =>
            (UpdateOnPremiseHandler)_scope.UpdateOnPremiseHandler;

        public UpdateHybridHostingTypeHandler UpdateHybridHostingTypeHandler =>
            (UpdateHybridHostingTypeHandler)_scope.UpdateHybridHostingTypeHandler;

        public UpdateRoadmapHandler UpdateRoadmapHandler =>
            (UpdateRoadmapHandler)_scope.UpdateRoadmapHandler;

        public UpdateSupplierHandler UpdateSupplierHandler =>
            (UpdateSupplierHandler)_scope.UpdateSupplierHandler;

        public UpdateIntegrationsHandler UpdateIntegrationsHandler =>
            (UpdateIntegrationsHandler)_scope.UpdateIntegrationsHandler;

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
            serviceCollection.AddSingleton(MockSolutionRepository.Object);
            MockSolutionDetailRepository = new Mock<ISolutionDetailRepository>();
            serviceCollection.AddSingleton(MockSolutionDetailRepository.Object);
            MockSolutionCapabilityRepository = new Mock<ISolutionCapabilityRepository>();
            serviceCollection.AddSingleton(MockSolutionCapabilityRepository.Object);
            MockMarketingContactRepository = new Mock<IMarketingContactRepository>();
            serviceCollection.AddSingleton(MockMarketingContactRepository.Object);
            MockSupplierRepository = new Mock<ISupplierRepository>();
            serviceCollection.AddSingleton(MockSupplierRepository.Object);
            MockDocumentRepository = new Mock<IDocumentRepository>();
            serviceCollection.AddSingleton(MockDocumentRepository.Object);
        }

        private class Scope
        {
            public IRequestHandler<GetSolutionByIdQuery, ISolution> GetSolutionByIdHandler { get; }

            public IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> GetClientApplicationBySolutionIdHandler { get; }

            public IRequestHandler<GetHostingBySolutionIdQuery, IHosting> GetHostingBySolutionIdHandler { get; }

            public IRequestHandler<GetSupplierBySolutionIdQuery, ISupplier> GetSupplierBySolutionIdHandler { get; }

            public IRequestHandler<GetRoadMapBySolutionIdQuery, IRoadMap> GetRoadMapBySolutionIdHandler { get; }

            public IRequestHandler<GetIntegrationsBySolutionIdQuery, IIntegrations> GetIntegrationsBySolutionIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand, ISimpleResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, ISimpleResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand, ISimpleResult> UpdateSolutionClientApplicationTypesHandler { get; }

            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand, ISimpleResult> UpdateSolutionBrowsersSupportedHandler { get; }

            public IRequestHandler<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult> UpdateSolutionContactDetailsHandler { get; }

            public IRequestHandler<UpdateSolutionPluginsCommand, ISimpleResult> UpdateSolutionPluginsHandler { get; }

            public IRequestHandler<GetContactDetailBySolutionIdQuery, IEnumerable<IContact>> GetContactDetailBySolutionIdHandler { get; }

            public IRequestHandler<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult> UpdateSolutionBrowserHardwareRequirementsHandler { get; }

            public IRequestHandler<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult> UpdateSolutionConnectivityAndResolutionHandler { get; }

            public IRequestHandler<UpdateBrowserBasedAdditionalInformationCommand, ISimpleResult> UpdateSolutionBrowserAdditionalInformationHandler { get; }

            public IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult> UpdateSolutionBrowserMobileFirstHandler { get; }

            public IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult> UpdateSolutionMobileOperatingSystemsHandler {get;}

            public IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult> UpdateSolutionMobileConnectionDetailsHandler {get;}

            public IRequestHandler<UpdateSolutionNativeMobileFirstCommand, ISimpleResult> UpdateSolutionNativeMobileFirstHandler { get; }

            public IRequestHandler<UpdateSolutionMobileMemoryStorageCommand, ISimpleResult> UpdateSolutionMobileMemoryStorageHandler { get; }

            public IRequestHandler<UpdateSolutionMobileThirdPartyCommand, ISimpleResult> UpdateSolutionMobileThirdHandler { get; }

            public IRequestHandler<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult> UpdateSolutionNativeMobileAdditionalInformationHandler { get; }

            public IRequestHandler<UpdateSolutionNativeMobileHardwareRequirementsCommand, ISimpleResult> UpdateSolutionNativeMobileHardwareRequirementsHandler { get; }

            public IRequestHandler<UpdateNativeDesktopHardwareRequirementsCommand, ISimpleResult> UpdateNativeDesktopHardwareRequirementsHandler { get; }

            public IRequestHandler<UpdateSolutionNativeDesktopConnectivityDetailsCommand, ISimpleResult> UpdateSolutionNativeDesktopConnectivityDetailsHandler { get; }

            public IRequestHandler<UpdateSolutionNativeDesktopOperatingSystemsCommand, ISimpleResult> UpdateSolutionNativeDesktopOperatingSystemsHandler { get; }

            public IRequestHandler<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult> UpdateSolutionNativeDesktopThirdPartyHandler { get; }

            public IRequestHandler<UpdateNativeDesktopMemoryAndStorageCommand, ISimpleResult> UpdateNativeDesktopMemoryAndStorageHandler { get; }

            public IRequestHandler<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult> UpdateNativeDesktopAdditionalInformationHandler { get; }

            public IRequestHandler<UpdatePublicCloudCommand, ISimpleResult> UpdatePublicCloudHandler { get; }

            public IRequestHandler<UpdatePrivateCloudCommand, ISimpleResult> UpdatePrivateCloudHandler { get; }

            public IRequestHandler<UpdateOnPremiseCommand, ISimpleResult> UpdateOnPremiseHandler { get; }

            public IRequestHandler<UpdateHybridHostingTypeCommand, ISimpleResult> UpdateHybridHostingTypeHandler { get; }

            public IRequestHandler<UpdateRoadmapCommand, ISimpleResult> UpdateRoadmapHandler { get; }

            public IRequestHandler<UpdateSupplierCommand, ISimpleResult> UpdateSupplierHandler { get; }

            public IRequestHandler<UpdateIntegrationsCommand, ISimpleResult> UpdateIntegrationsHandler { get; }

            public Scope(IRequestHandler<GetSolutionByIdQuery, ISolution> getSolutionByIdHandler,
                IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> getClientApplicationBySolutionIdHandler,
                IRequestHandler<GetHostingBySolutionIdQuery, IHosting> getHostingBySolutionIdHandler,
                IRequestHandler<GetSupplierBySolutionIdQuery, ISupplier> getSupplierBySolutionIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand, ISimpleResult> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand, ISimpleResult> updateSolutionFeaturesHandler,
                IRequestHandler<UpdateSolutionClientApplicationTypesCommand, ISimpleResult> updateSolutionClientApplicationTypesHandler,
                IRequestHandler<UpdateSolutionBrowsersSupportedCommand, ISimpleResult> updateSolutionBrowsersSupportedHandler,
                IRequestHandler<UpdateSolutionPluginsCommand, ISimpleResult> updateSolutionPluginsHandler,
                IRequestHandler<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult> updateSolutionContactDetailsHandler,
                IRequestHandler<GetContactDetailBySolutionIdQuery, IEnumerable<IContact>> getContactDetailBySolutionIdHandler,
                IRequestHandler<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult> updateSolutionBrowserHardwareRequirementsHandler,
                IRequestHandler<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult> updateSolutionConnectivityAndResolutionHandler,
                IRequestHandler<UpdateBrowserBasedAdditionalInformationCommand, ISimpleResult> updateSolutionBrowserAdditionalInformationHandler,
                IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult> updateSolutionBrowserMobileFirstHandler,
                IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult> updateSolutionMobileOperatingSystemsHandler,
                IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult> updateSolutionMobileConnectionDetailsHandler,
                IRequestHandler<UpdateSolutionNativeMobileFirstCommand, ISimpleResult> updateSolutionNativeMobileFirstHandler,
                IRequestHandler<UpdateSolutionMobileMemoryStorageCommand, ISimpleResult> updateSolutionMobileMemoryStorageHandler,
                IRequestHandler<UpdateSolutionNativeMobileHardwareRequirementsCommand, ISimpleResult> updateSolutionNativeMobileHardwareRequirementsHandler,
                IRequestHandler<UpdateNativeDesktopHardwareRequirementsCommand, ISimpleResult> updateNativeDesktopHardwareRequirementsHandler,
                IRequestHandler<UpdateSolutionMobileThirdPartyCommand, ISimpleResult> updateSolutionMobileThirdHandler,
                IRequestHandler<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult> updateSolutionNativeMobileAdditionalInformationHandler,
                IRequestHandler<UpdateSolutionNativeDesktopConnectivityDetailsCommand, ISimpleResult> updateSolutionNativeDesktopConnectivityDetailsHandler,
                IRequestHandler<UpdateSolutionNativeDesktopOperatingSystemsCommand, ISimpleResult> updateSolutionNativeDesktopOperatingSystemsHandler,
                IRequestHandler<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult> updateSolutionNativeDesktopThirdPartyHandler,
                IRequestHandler<UpdateNativeDesktopMemoryAndStorageCommand, ISimpleResult> updateNativeDestkopMemoryAndStorageHandler,
                IRequestHandler<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult> updateNativeDesktopAdditionalInformationHandler,
                IRequestHandler<UpdatePublicCloudCommand, ISimpleResult> updatePublicCloudHandler,
                IRequestHandler<UpdatePrivateCloudCommand, ISimpleResult> updatePrivateCloudHandler,
                IRequestHandler<UpdateOnPremiseCommand, ISimpleResult> updateOnPremiseHandler,
                IRequestHandler<UpdateHybridHostingTypeCommand, ISimpleResult> updateHybridHostingTypeHandler,
                IRequestHandler<UpdateRoadmapCommand, ISimpleResult> updateRoadmapHandler,
                IRequestHandler<GetRoadMapBySolutionIdQuery, IRoadMap> getRoadMapBySolutionIdHandler,
                IRequestHandler<GetIntegrationsBySolutionIdQuery, IIntegrations> getIntegrationsBySolutionIdHandler,
                IRequestHandler<UpdateSupplierCommand, ISimpleResult> updateSupplierRequestHandler,
                IRequestHandler<UpdateIntegrationsCommand, ISimpleResult> updateIntegrationsRequestHandler)
            {
                GetSolutionByIdHandler = getSolutionByIdHandler;
                GetClientApplicationBySolutionIdHandler = getClientApplicationBySolutionIdHandler;
                GetHostingBySolutionIdHandler = getHostingBySolutionIdHandler;
                GetSupplierBySolutionIdHandler = getSupplierBySolutionIdHandler;
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
                UpdateSolutionNativeMobileFirstHandler = updateSolutionNativeMobileFirstHandler;
                UpdateSolutionMobileMemoryStorageHandler = updateSolutionMobileMemoryStorageHandler;
                UpdateSolutionNativeMobileHardwareRequirementsHandler = updateSolutionNativeMobileHardwareRequirementsHandler;
                UpdateNativeDesktopHardwareRequirementsHandler = updateNativeDesktopHardwareRequirementsHandler;
                UpdateSolutionMobileThirdHandler = updateSolutionMobileThirdHandler;
                UpdateSolutionNativeMobileAdditionalInformationHandler = updateSolutionNativeMobileAdditionalInformationHandler;
                UpdateSolutionNativeDesktopOperatingSystemsHandler = updateSolutionNativeDesktopOperatingSystemsHandler;
                UpdateSolutionNativeDesktopConnectivityDetailsHandler = updateSolutionNativeDesktopConnectivityDetailsHandler;
                UpdateSolutionNativeDesktopThirdPartyHandler = updateSolutionNativeDesktopThirdPartyHandler;
                UpdateNativeDesktopMemoryAndStorageHandler = updateNativeDestkopMemoryAndStorageHandler;
                UpdateNativeDesktopAdditionalInformationHandler = updateNativeDesktopAdditionalInformationHandler;
                UpdatePublicCloudHandler = updatePublicCloudHandler;
                UpdatePrivateCloudHandler = updatePrivateCloudHandler;
                UpdateOnPremiseHandler = updateOnPremiseHandler;
                UpdateHybridHostingTypeHandler = updateHybridHostingTypeHandler;
                UpdateRoadmapHandler = updateRoadmapHandler;
                GetRoadMapBySolutionIdHandler = getRoadMapBySolutionIdHandler;
                GetIntegrationsBySolutionIdHandler = getIntegrationsBySolutionIdHandler;
                UpdateSupplierHandler = updateSupplierRequestHandler;
                UpdateIntegrationsHandler = updateIntegrationsRequestHandler;
            }
        }
    }
}

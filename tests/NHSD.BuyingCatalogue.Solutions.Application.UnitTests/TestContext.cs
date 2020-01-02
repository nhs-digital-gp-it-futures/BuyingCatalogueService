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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileFirst;
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

        public UpdateSolutionNativeMobileFirstHandler UpdateSolutionNativeMobileFirstHandler =>
            (UpdateSolutionNativeMobileFirstHandler)_scope.UpdateSolutionNativeMobileFirstHandler;

        public UpdateSolutionMobileMemoryStorageHandler UpdateSolutionMobileMemoryStorageHandler =>
            (UpdateSolutionMobileMemoryStorageHandler)_scope.UpdateSolutionMobileMemoryStorageHandler;

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

            public IRequestHandler<UpdateSolutionSummaryCommand, ISimpleResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, ISimpleResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand, ISimpleResult> UpdateSolutionClientApplicationTypesHandler { get; }

            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand, ISimpleResult> UpdateSolutionBrowsersSupportedHandler { get; }

            public IRequestHandler<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult> UpdateSolutionContactDetailsHandler { get; }

            public IRequestHandler<UpdateSolutionPluginsCommand, ISimpleResult> UpdateSolutionPluginsHandler { get; }

            public IRequestHandler<GetContactDetailBySolutionIdQuery, IEnumerable<IContact>> GetContactDetailBySolutionIdHandler { get; }

            public IRequestHandler<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult> UpdateSolutionBrowserHardwareRequirementsHandler { get; }

            public IRequestHandler<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult> UpdateSolutionConnectivityAndResolutionHandler { get; }
            public IRequestHandler<UpdateSolutionBrowserAdditionalInformationCommand, ISimpleResult> UpdateSolutionBrowserAdditionalInformationHandler { get; }

            public IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult> UpdateSolutionBrowserMobileFirstHandler { get; }

            public IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult> UpdateSolutionMobileOperatingSystemsHandler {get;}

            public IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult> UpdateSolutionMobileConnectionDetailsHandler {get;}

            public IRequestHandler<UpdateSolutionNativeMobileFirstCommand, ISimpleResult> UpdateSolutionNativeMobileFirstHandler { get; }

            public IRequestHandler<UpdateSolutionMobileMemoryStorageCommand, ISimpleResult> UpdateSolutionMobileMemoryStorageHandler { get; }
           

            public Scope(IRequestHandler<GetSolutionByIdQuery, ISolution> getSolutionByIdHandler,
                IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication> getClientApplicationBySolutionIdHandler,
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
                IRequestHandler<UpdateSolutionBrowserAdditionalInformationCommand, ISimpleResult> updateSolutionBrowserAdditionalInformationHandler,
                IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult> updateSolutionBrowserMobileFirstHandler,
                IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult> updateSolutionMobileOperatingSystemsHandler,
                IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult> updateSolutionMobileConnectionDetailsHandler,
                IRequestHandler<UpdateSolutionNativeMobileFirstCommand, ISimpleResult> updateSolutionNativeMobileFirstHandler,
                IRequestHandler<UpdateSolutionMobileMemoryStorageCommand, ISimpleResult> updateSolutionMobileMemoryStorageHandler
                )
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
                UpdateSolutionNativeMobileFirstHandler = updateSolutionNativeMobileFirstHandler;
                UpdateSolutionMobileMemoryStorageHandler = updateSolutionMobileMemoryStorageHandler;
            }
        }
    }
}

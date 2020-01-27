using Microsoft.Extensions.DependencyInjection;
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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.HybridHostingType;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<SolutionReader>()
                .AddTransient<ClientApplicationReader>()
                .AddTransient<HostingReader>()
                .AddTransient<SupplierReader>()
                .AddTransient<ContactDetailsReader>()
                .AddTransient<RoadMapReader>()
                .AddTransient<SolutionVerifier>()
                .AddTransient<SolutionSummaryUpdater>()
                .AddTransient<SolutionFeaturesUpdater>()
                .AddTransient<SolutionClientApplicationUpdater>()
                .AddTransient<SolutionHostingUpdater>()
                .AddTransient<SolutionRoadmapUpdater>()
                .AddTransient<SolutionContactDetailsUpdater>()
                .AddTransient<ClientApplicationPartialUpdater>()
                .AddTransient<HostingPartialUpdater>()
                .AddTransient<SupplierUpdater>()
                .AddTransient<SupplierPartialUpdater>()
                .AddTransient<UpdateSolutionFeaturesValidator>()
                .AddTransient<UpdateSolutionClientApplicationTypesValidator>()
                .AddTransient<UpdateSolutionBrowsersSupportedValidator>()
                .AddTransient<UpdateSolutionContactDetailsValidator>()
                .AddTransient<UpdateSolutionBrowserHardwareRequirementsValidator>()
                .AddTransient<UpdateSolutionConnectivityAndResolutionValidator>()
                .AddTransient<UpdateBrowserBasedAdditionalInformationValidator>()
                .AddTransient<UpdateSolutionBrowserMobileFirstValidator>()
                .AddTransient<UpdateSolutionMobileOperatingSystemsValidator>()
                .AddTransient<UpdateSolutionMobileConnectionDetailsValidator>()
				.AddTransient<UpdateSolutionNativeMobileFirstValidator>()
                .AddTransient<UpdateSupplierValidator>()

                .AddTransient<IExecutor<UpdateSolutionSummaryCommand>, UpdateSolutionSummaryExecutor>()
                .AddTransient<IValidator<UpdateSolutionSummaryCommand, ISimpleResult>, UpdateSolutionSummaryValidator>()

                .AddTransient<IExecutor<UpdateSolutionPluginsCommand>, UpdateSolutionPluginsExecutor>()
                .AddTransient<IValidator<UpdateSolutionPluginsCommand, ISimpleResult>, UpdateSolutionPluginsValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileOperatingSystemsCommand>, UpdateSolutionMobileOperatingSystemsExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult>, UpdateSolutionMobileOperatingSystemsValidator>()

                .AddTransient<IExecutor<UpdateSolutionContactDetailsCommand>, UpdateSolutionContactDetailsExecutor>()
                .AddTransient<IValidator<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult>, UpdateSolutionContactDetailsValidator>()

                .AddTransient<IExecutor<UpdateSolutionFeaturesCommand>, UpdateSolutionFeaturesExecutor>()
                .AddTransient<IValidator<UpdateSolutionFeaturesCommand, ISimpleResult>, UpdateSolutionFeaturesValidator>()

                .AddTransient<IExecutor<UpdateSolutionConnectivityAndResolutionCommand>, UpdateSolutionConnectivityAndResolutionExecutor>()
                .AddTransient<IValidator<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult>, UpdateSolutionConnectivityAndResolutionValidator>()

                .AddTransient<IExecutor<UpdateSolutionClientApplicationTypesCommand>, UpdateSolutionClientApplicationTypesExecutor>()
                .AddTransient<IValidator<UpdateSolutionClientApplicationTypesCommand, ISimpleResult>, UpdateSolutionClientApplicationTypesValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowsersSupportedCommand>, UpdateSolutionBrowsersSupportedExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowsersSupportedCommand, ISimpleResult>, UpdateSolutionBrowsersSupportedValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileConnectionDetailsCommand>, UpdateSolutionMobileConnectionDetailsExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult>, UpdateSolutionMobileConnectionDetailsValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowserMobileFirstCommand>, UpdateSolutionBrowserMobileFirstExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult>, UpdateSolutionBrowserMobileFirstValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowserHardwareRequirementsCommand>, UpdateSolutionBrowserHardwareRequirementsExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult>, UpdateSolutionBrowserHardwareRequirementsValidator>()

                .AddTransient<IExecutor<UpdateBrowserBasedAdditionalInformationCommand>, UpdateBrowserBasedAdditionalInformationExecutor>()
                .AddTransient<IValidator<UpdateBrowserBasedAdditionalInformationCommand, ISimpleResult>, UpdateBrowserBasedAdditionalInformationValidator>()

                .AddTransient<IExecutor<UpdateSolutionNativeMobileAdditionalInformationCommand>, UpdateSolutionNativeMobileAdditionalInformationExecutor>()
                .AddTransient<IValidator<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult>, UpdateSolutionNativeMobileAdditionalInformationValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileMemoryStorageCommand>, UpdateSolutionMobileMemoryStorageExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileMemoryStorageCommand, ISimpleResult>, UpdateSolutionMobileMemoryStorageValidator>()

                .AddTransient<IExecutor<UpdateSolutionNativeMobileHardwareRequirementsCommand>, UpdateSolutionNativeMobileHardwareRequirementsExecutor>()
                .AddTransient<IValidator<UpdateSolutionNativeMobileHardwareRequirementsCommand, ISimpleResult>, UpdateSolutionNativeMobileHardwareRequirementsValidator>()

                .AddTransient<IExecutor<UpdateNativeDesktopHardwareRequirementsCommand>, UpdateNativeDesktopHardwareRequirementsExecutor>()
                .AddTransient<IValidator<UpdateNativeDesktopHardwareRequirementsCommand, ISimpleResult>, UpdateNativeDesktopHardwareRequirementsValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileThirdPartyCommand>, UpdateSolutionMobileThirdPartyExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileThirdPartyCommand, ISimpleResult>, UpdateSolutionMobileThirdPartyValidator>()

                .AddTransient<IExecutor<UpdateSolutionNativeDesktopConnectivityDetailsCommand>, UpdateSolutionNativeDesktopConnectivityDetailsExecutor>()
                .AddTransient<IValidator<UpdateSolutionNativeDesktopConnectivityDetailsCommand, ISimpleResult>, UpdateSolutionNativeDesktopConnectivityDetailsValidator>()

                .AddTransient<IExecutor<UpdateSolutionNativeDesktopOperatingSystemsCommand>, UpdateSolutionNativeDesktopOperatingSystemsExecutor>()
                .AddTransient<IValidator<UpdateSolutionNativeDesktopOperatingSystemsCommand, ISimpleResult>, UpdateSolutionNativeDesktopOperatingSystemsValidator>()

                .AddTransient<IExecutor<UpdateNativeDesktopMemoryAndStorageCommand>, UpdateNativeDesktopMemoryAndStorageExecutor>()
                .AddTransient<IValidator<UpdateNativeDesktopMemoryAndStorageCommand, ISimpleResult>, UpdateNativeDesktopMemoryAndStorageValidator>()

                .AddTransient<IExecutor<UpdateSolutionNativeDesktopThirdPartyCommand>, UpdateSolutionNativeDesktopThirdPartyExecutor>()
                .AddTransient<IValidator<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult>, UpdateSolutionNativeDesktopThirdPartyValidator>()

                .AddTransient<IExecutor<UpdateNativeDesktopAdditionalInformationCommand>, UpdateNativeDesktopAdditionalInformationExecutor>()
                .AddTransient<IValidator<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult>, UpdateNativeDesktopAdditionalInformationValidator>()

                .AddTransient<IExecutor<UpdatePublicCloudCommand>, UpdatePublicCloudExecutor>()
                .AddTransient<IValidator<UpdatePublicCloudCommand, ISimpleResult>, UpdatePublicCloudValidator>()

                .AddTransient<IExecutor<UpdatePrivateCloudCommand>, UpdatePrivateCloudExecutor>()
                .AddTransient<IValidator<UpdatePrivateCloudCommand, ISimpleResult>, UpdatePrivateCloudValidator>()

                .AddTransient<IExecutor<UpdateOnPremiseCommand>, UpdateOnPremiseExecutor>()
                .AddTransient<IValidator<UpdateOnPremiseCommand, ISimpleResult>, UpdateOnPremiseValidator>()

                .AddTransient<IExecutor<UpdateHybridHostingTypeCommand>, UpdateHybridHostingTypeExecutor>()
                .AddTransient<IValidator<UpdateHybridHostingTypeCommand, ISimpleResult>, UpdateHybridHostingTypeValidator>()

                .AddTransient<IExecutor<UpdateRoadmapCommand>, UpdateRoadmapExecutor>()
                .AddTransient<IValidator<UpdateRoadmapCommand, ISimpleResult>, UpdateRoadmapValidator>()

                .AddTransient<IExecutor<UpdateSupplierCommand>, UpdateSupplierExecutor>()
                .AddTransient<IValidator<UpdateSupplierCommand, ISimpleResult>, UpdateSupplierValidator>()
                ;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateBrowserBasedAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSolutionApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<SolutionReader>()
                .AddTransient<ClientApplicationReader>()
                .AddTransient<ContactDetailsReader>()
                .AddTransient<SolutionVerifier>()
                .AddTransient<SolutionSummaryUpdater>()
                .AddTransient<SolutionFeaturesUpdater>()
                .AddTransient<SolutionClientApplicationUpdater>()
                .AddTransient<SolutionContactDetailsUpdater>()
                .AddTransient<ClientApplicationPartialUpdater>()
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
                .AddTransient<IValidator<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult>, UpdateSolutionNativeDesktopThirdPartyValidator>();
        }
    }
}

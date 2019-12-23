using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileFirst;

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
                .AddTransient<UpdateSolutionBrowserAdditionalInformationValidator>()
                .AddTransient<UpdateSolutionBrowserMobileFirstValidator>()
                .AddTransient<UpdateSolutionMobileOperatingSystemsValidator>()
                .AddTransient<UpdateSolutionMobileConnectionDetailsValidator>()
				.AddTransient<UpdateSolutionNativeMobileFirstValidator>()

                .AddTransient<IExecutor<UpdateSolutionSummaryCommand>, UpdateSolutionSummaryExecutor>()
                .AddTransient<IValidator<UpdateSolutionSummaryCommand, RequiredMaxLengthResult>, UpdateSolutionSummaryValidator>()

                .AddTransient<IExecutor<UpdateSolutionPluginsCommand>, UpdateSolutionPluginsExecutor>()
                .AddTransient<IValidator<UpdateSolutionPluginsCommand, RequiredMaxLengthResult>, UpdateSolutionPluginsValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileOperatingSystemsCommand>, UpdateSolutionMobileOperatingSystemsExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileOperatingSystemsCommand, RequiredMaxLengthResult>, UpdateSolutionMobileOperatingSystemsValidator>()

                .AddTransient<IExecutor<UpdateSolutionContactDetailsCommand>, UpdateSolutionContactDetailsExecutor>()
                .AddTransient<IValidator<UpdateSolutionContactDetailsCommand, MaxLengthResult>, UpdateSolutionContactDetailsValidator>()

                .AddTransient<IExecutor<UpdateSolutionFeaturesCommand>, UpdateSolutionFeaturesExecutor>()
                .AddTransient<IValidator<UpdateSolutionFeaturesCommand, MaxLengthResult>, UpdateSolutionFeaturesValidator>()

                .AddTransient<IExecutor<UpdateSolutionConnectivityAndResolutionCommand>, UpdateSolutionConnectivityAndResolutionExecutor>()
                .AddTransient<IValidator<UpdateSolutionConnectivityAndResolutionCommand, RequiredResult>, UpdateSolutionConnectivityAndResolutionValidator>()

                .AddTransient<IExecutor<UpdateSolutionClientApplicationTypesCommand>, UpdateSolutionClientApplicationTypesExecutor>()
                .AddTransient<IValidator<UpdateSolutionClientApplicationTypesCommand, RequiredResult>, UpdateSolutionClientApplicationTypesValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowsersSupportedCommand>, UpdateSolutionBrowsersSupportedExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowsersSupportedCommand, RequiredResult>, UpdateSolutionBrowsersSupportedValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileConnectionDetailsCommand>, UpdateSolutionMobileConnectionDetailsExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult>, UpdateSolutionMobileConnectionDetailsValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowserMobileFirstCommand>, UpdateSolutionBrowserMobileFirstExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowserMobileFirstCommand, RequiredResult>, UpdateSolutionBrowserMobileFirstValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowserHardwareRequirementsCommand>, UpdateSolutionBrowserHardwareRequirementsExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowserHardwareRequirementsCommand, MaxLengthResult>, UpdateSolutionBrowserHardwareRequirementsValidator>()

                .AddTransient<IExecutor<UpdateSolutionBrowserAdditionalInformationCommand>, UpdateSolutionBrowserAdditionalInformationExecutor>()
                .AddTransient<IValidator<UpdateSolutionBrowserAdditionalInformationCommand, MaxLengthResult>, UpdateSolutionBrowserAdditionalInformationValidator>()

                .AddTransient<IExecutor<UpdateSolutionMobileMemoryStorageCommand>, UpdateSolutionMobileMemoryStorageExecutor>()
                .AddTransient<IValidator<UpdateSolutionMobileMemoryStorageCommand, RequiredMaxLengthResult>, UpdateSolutionMobileMemoryStorageValidator>()
                ;
        }
    }
}

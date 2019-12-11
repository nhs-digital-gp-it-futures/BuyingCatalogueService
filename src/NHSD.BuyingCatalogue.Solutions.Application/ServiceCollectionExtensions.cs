using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
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
                .AddTransient<UpdateSolutionSummaryValidator>()
                .AddTransient<UpdateSolutionFeaturesValidator>()
                .AddTransient<UpdateSolutionClientApplicationTypesValidator>()
                .AddTransient<UpdateSolutionBrowsersSupportedValidator>()
                .AddTransient<UpdateSolutionContactDetailsValidator>()
                .AddTransient<UpdateSolutionPluginsValidator>()
                .AddTransient<UpdateSolutionBrowserHardwareRequirementsValidator>()
                .AddTransient<UpdateSolutionConnectivityAndResolutionValidator>()
                .AddTransient<UpdateSolutionBrowserAdditionalInformationValidator>();
        }
    }
}

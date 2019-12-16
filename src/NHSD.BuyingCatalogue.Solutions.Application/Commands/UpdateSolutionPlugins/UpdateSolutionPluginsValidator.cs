using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator
    {
        public RequiredMaxLengthResult Validation(UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
        {
            var validationResult = new RequiredMaxLengthResult();

            if (string.IsNullOrWhiteSpace(updateSolutionPluginsViewModel.ThrowIfNull(nameof(updateSolutionPluginsViewModel)).Required))
            {
                validationResult.Required.Add("plugins-required");
            }
            if (updateSolutionPluginsViewModel.AdditionalInformation?.Length > 500)
            {
                validationResult.MaxLength.Add("plugins-detail");
            }

            return validationResult;
        }
    }
}

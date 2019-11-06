using System.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator
    {
        public UpdateSolutionPluginsValidationResult Validation(
            UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
        {
            var validationResult = new UpdateSolutionPluginsValidationResult();

            if (string.IsNullOrWhiteSpace(updateSolutionPluginsViewModel.Required))
            {
                validationResult.Required.Add("plugins-required");
            }

            if (string.IsNullOrWhiteSpace(updateSolutionPluginsViewModel.AdditionalInformation))
            {
                validationResult.MaxLength.Add("plugins-detail");
            }

            return validationResult;
        }
    }
}

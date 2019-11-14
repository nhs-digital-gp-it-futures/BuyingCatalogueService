using System;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator
    {
        public UpdateSolutionPluginsValidationResult Validation(
            UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
        {
            if (updateSolutionPluginsViewModel == null)
            {
                throw new ArgumentNullException(nameof(updateSolutionPluginsViewModel));
            }

            var validationResult = new UpdateSolutionPluginsValidationResult();

            if (string.IsNullOrWhiteSpace(updateSolutionPluginsViewModel.Required))
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

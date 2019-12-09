using System;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator
    {
        public UpdateSolutionPluginsValidationResult Validation(UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
        {
            var validationResult = new UpdateSolutionPluginsValidationResult();

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

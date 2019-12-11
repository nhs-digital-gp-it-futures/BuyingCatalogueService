using System;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionValidator
    {
        public UpdateSolutionConnectivityAndResolutionValidationResult Validation(UpdateSolutionConnectivityAndResolutionViewModel viewModel)
        {
            var result = new UpdateSolutionConnectivityAndResolutionValidationResult();
            if (String.IsNullOrWhiteSpace(viewModel.MinimumConnectionSpeed))
            {
                result.Required.Add("minimum-connection-speed");
            }

            if (String.IsNullOrWhiteSpace(viewModel.MinimumDesktopResolution))
            {
                result.Required.Add("minimum-desktop-resolution");
            }

            return result;
        }
    }
}

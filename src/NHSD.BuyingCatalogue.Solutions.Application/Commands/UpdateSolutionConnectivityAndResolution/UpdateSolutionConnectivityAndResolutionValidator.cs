using System;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionValidator
    {
        public RequiredResult Validation(UpdateSolutionConnectivityAndResolutionViewModel viewModel)
        {
            var result = new RequiredResult();
            if (String.IsNullOrWhiteSpace(viewModel.MinimumConnectionSpeed))
            {
                result.Required.Add("minimum-connection-speed");
            }

            return result;
        }
    }
}

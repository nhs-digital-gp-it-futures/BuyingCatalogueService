using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class UpdateSolutionBrowserSupportedResult
    {
        public UpdateSolutionBrowserSupportedResult(UpdateSolutionBrowserSupportedValidationResult updateSolutionBrowserSupportedValidationResult)
        {
            Required = updateSolutionBrowserSupportedValidationResult.Required;
        }

        public HashSet<string> Required { get; }
    }
}

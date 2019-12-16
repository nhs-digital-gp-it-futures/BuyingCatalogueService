using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionBrowserSupportedResult
    {
        internal UpdateSolutionBrowserSupportedResult(RequiredResult updateSolutionBrowserSupportedValidationResult)
        {
            Required = updateSolutionBrowserSupportedValidationResult.Required;
        }

        public HashSet<string> Required { get; }
    }
}

using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionBrowserMobileFirstResult
    {
        internal UpdateSolutionBrowserMobileFirstResult(UpdateSolutionBrowserMobileFirstValidationResult validationResult)
            => Required = validationResult.Required;

        public HashSet<string> Required { get; }
    }
}

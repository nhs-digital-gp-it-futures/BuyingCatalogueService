using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedValidator
    {
        public RequiredResult Validation(
            UpdateSolutionBrowsersSupportedViewModel updateSolutionBrowsersSupportedViewModel)
        {
            var validationResult = new RequiredResult();

            if (!updateSolutionBrowsersSupportedViewModel.BrowsersSupported.Any())
            {
                validationResult.Required.Add("supported-browsers");
            }

            if (string.IsNullOrWhiteSpace(updateSolutionBrowsersSupportedViewModel.MobileResponsive))
            {
                validationResult.Required.Add("mobile-responsive");
            }

            return validationResult;
        }
    }
}

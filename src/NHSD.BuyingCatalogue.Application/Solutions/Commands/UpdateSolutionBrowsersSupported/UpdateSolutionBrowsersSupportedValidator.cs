using System.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedValidator
    {
        public UpdateSolutionBrowserSupportedValidationResult Validation(
            UpdateSolutionBrowsersSupportedViewModel updateSolutionBrowsersSupportedViewModel)
        {
            var validationResult = new UpdateSolutionBrowserSupportedValidationResult();

            if (!updateSolutionBrowsersSupportedViewModel.BrowsersSupported.Any())
            {
                validationResult.Required.Add("browsers-supported");
            }

            if (string.IsNullOrWhiteSpace(updateSolutionBrowsersSupportedViewModel.MobileResponsive))
            {
                validationResult.Required.Add("mobile-responsive");
            }

            return validationResult;
        }
    }
}

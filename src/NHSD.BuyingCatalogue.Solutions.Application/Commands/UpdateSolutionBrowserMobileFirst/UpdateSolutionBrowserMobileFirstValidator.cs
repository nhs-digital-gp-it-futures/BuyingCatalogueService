using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstValidator
    {
        public RequiredResult Validation(UpdateSolutionBrowserMobileFirstViewModel updateSolutionBrowserMobileFirstViewModel)
        {
            var validationResult = new RequiredResult();

            if (string.IsNullOrEmpty(updateSolutionBrowserMobileFirstViewModel.MobileFirstDesign))
            {
                validationResult.Required.Add("mobile-first-design");
            }

            return validationResult;
        }
    }
}

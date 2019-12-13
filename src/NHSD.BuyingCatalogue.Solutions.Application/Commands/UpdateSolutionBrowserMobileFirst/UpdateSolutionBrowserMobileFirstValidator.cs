namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstValidator
    {
        public UpdateSolutionBrowserMobileFirstValidationResult Validation(UpdateSolutionBrowserMobileFirstViewModel updateSolutionBrowserMobileFirstViewModel)
        {
            var validationResult = new UpdateSolutionBrowserMobileFirstValidationResult();

            if (string.IsNullOrEmpty(updateSolutionBrowserMobileFirstViewModel.MobileFirstDesign))
            {
                validationResult.Required.Add("mobile-first-design");
            }

            return validationResult;
        }
    }
}

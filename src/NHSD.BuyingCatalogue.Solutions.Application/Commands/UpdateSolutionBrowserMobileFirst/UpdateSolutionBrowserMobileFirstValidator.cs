using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstValidator
    {
        public RequiredResult Validation(UpdateSolutionBrowserMobileFirstViewModel updateSolutionBrowserMobileFirstViewModel)
            => new RequiredValidator()
                .Validate(updateSolutionBrowserMobileFirstViewModel.MobileFirstDesign, "mobile-first-design")
                .Result();
    }
}

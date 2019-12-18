using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileFirst
{
    internal sealed class UpdateSolutionNativeMobileFirstValidator
    {
        public RequiredResult Validation(UpdateSolutionNativeMobileFirstViewModel updateSolutionNativeMobileFirstViewModel)
            => new RequiredValidator()
                .Validate(updateSolutionNativeMobileFirstViewModel.MobileFirstDesign, "mobile-first-design")
                .Result();
    }
}

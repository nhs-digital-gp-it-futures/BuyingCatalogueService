using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst
{
    internal sealed class UpdateSolutionNativeMobileFirstValidator
    {
        public ISimpleResult Validation(UpdateSolutionNativeMobileFirstViewModel updateSolutionNativeMobileFirstViewModel)
            => new RequiredValidator()
                .Validate(updateSolutionNativeMobileFirstViewModel.MobileFirstDesign, "mobile-first-design")
                .Result();
    }
}

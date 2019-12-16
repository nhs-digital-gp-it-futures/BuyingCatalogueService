using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationValidator
    {
        public MaxLengthResult Validation(UpdateSolutionBrowserAdditionalInformationViewModel updateSolutionBrowserAdditionalInformationViewModel)
         => new MaxLengthValidator()
                .Validate(updateSolutionBrowserAdditionalInformationViewModel.AdditionalInformation, 500, "additional-information")
                .Result();
    }
}

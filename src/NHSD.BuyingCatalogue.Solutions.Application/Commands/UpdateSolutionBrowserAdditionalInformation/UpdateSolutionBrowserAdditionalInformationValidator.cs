using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationValidator : IValidator<UpdateSolutionBrowserAdditionalInformationCommand, MaxLengthResult>
    {
        public MaxLengthResult Validate(UpdateSolutionBrowserAdditionalInformationCommand updateSolutionBrowserAdditionalInformationCommand)
         => new MaxLengthValidator()
                .Validate(updateSolutionBrowserAdditionalInformationCommand.UpdateSolutionBrowserAdditionalInformationViewModel.AdditionalInformation, 500, "additional-information")
                .Result();
    }
}

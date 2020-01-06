using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationValidator : IValidator<UpdateSolutionBrowserAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionBrowserAdditionalInformationCommand updateSolutionBrowserAdditionalInformationCommand)
         => new MaxLengthValidator()
                .Validate(updateSolutionBrowserAdditionalInformationCommand.Data.AdditionalInformation, 500, "additional-information")
                .Result();
    }
}

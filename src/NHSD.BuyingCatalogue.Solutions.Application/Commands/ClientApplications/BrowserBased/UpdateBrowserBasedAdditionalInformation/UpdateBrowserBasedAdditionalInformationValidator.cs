using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation
{
    internal sealed class UpdateBrowserBasedAdditionalInformationValidator : IValidator<UpdateBrowserBasedAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateBrowserBasedAdditionalInformationCommand updateSolutionBrowserAdditionalInformationCommand)
         => new MaxLengthValidator()
                .Validate(updateSolutionBrowserAdditionalInformationCommand.AdditionalInformation, 500, "additional-information")
                .Result();
    }
}

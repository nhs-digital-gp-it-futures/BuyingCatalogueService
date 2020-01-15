using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation
{
    internal sealed class UpdateSolutionNativeMobileAdditionalInformationValidator : IValidator<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeMobileAdditionalInformationCommand updateSolutionNativeMobileAdditionalInformationCommand)
         => new MaxLengthValidator()
                .Validate(updateSolutionNativeMobileAdditionalInformationCommand.Data.NativeMobileAdditionalInformation, 500, "additional-information")
                .Result();
    }
}

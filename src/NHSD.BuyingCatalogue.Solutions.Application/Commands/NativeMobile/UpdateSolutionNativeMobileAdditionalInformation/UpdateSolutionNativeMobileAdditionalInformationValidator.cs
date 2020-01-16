using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation
{
    internal sealed class UpdateSolutionNativeMobileAdditionalInformationValidator : IValidator<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeMobileAdditionalInformationCommand command)
         => new MaxLengthValidator()
                .Validate(command.Data.NativeMobileAdditionalInformation, 500, "additional-information")
                .Result();
    }
}

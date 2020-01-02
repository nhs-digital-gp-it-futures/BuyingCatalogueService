using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileAdditionalInformation
{
    internal sealed class UpdateSolutionNativeMobileAdditionalInformationValidator : IValidator<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeMobileAdditionalInformationCommand updateSolutionNativeMobileAdditionalInformationCommand)
         => new MaxLengthValidator()
                .Validate(updateSolutionNativeMobileAdditionalInformationCommand.UpdateSolutionNativeMobileAdditionalInformationViewModel.NativeMobileAdditionalInformation, 500, "additional-information")
                .Result();
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileAdditionalInformation
{
    internal sealed class UpdateSolutionNativeMobileAdditionalInformationHandler : Handler<UpdateSolutionNativeMobileAdditionalInformationCommand, MaxLengthResult>
    {
        public UpdateSolutionNativeMobileAdditionalInformationHandler(IExecutor<UpdateSolutionNativeMobileAdditionalInformationCommand> updateSolutionNativeMobileAdditionalInformationExecutor,
            IValidator<UpdateSolutionNativeMobileAdditionalInformationCommand, MaxLengthResult> updateSolutionNativeMobileAdditionalInformationValidator) : base(updateSolutionNativeMobileAdditionalInformationExecutor, updateSolutionNativeMobileAdditionalInformationValidator)
        {
        }
    }
}

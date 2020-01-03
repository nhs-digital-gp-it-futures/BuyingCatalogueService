using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileAdditionalInformation
{
    internal sealed class UpdateSolutionNativeMobileAdditionalInformationHandler : Handler<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult>
    {
        public UpdateSolutionNativeMobileAdditionalInformationHandler(IExecutor<UpdateSolutionNativeMobileAdditionalInformationCommand> updateSolutionNativeMobileAdditionalInformationExecutor,
            IValidator<UpdateSolutionNativeMobileAdditionalInformationCommand, ISimpleResult> updateSolutionNativeMobileAdditionalInformationValidator) : base(updateSolutionNativeMobileAdditionalInformationExecutor, updateSolutionNativeMobileAdditionalInformationValidator)
        {
        }
    }
}

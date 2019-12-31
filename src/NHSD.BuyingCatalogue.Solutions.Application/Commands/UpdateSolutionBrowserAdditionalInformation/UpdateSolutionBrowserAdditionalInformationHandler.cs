using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationHandler : Handler<UpdateSolutionBrowserAdditionalInformationCommand, ISimpleResult>
    {
        public UpdateSolutionBrowserAdditionalInformationHandler(IExecutor<UpdateSolutionBrowserAdditionalInformationCommand> updateSolutionBrowserAdditionalInformationExecutor,
            IValidator<UpdateSolutionBrowserAdditionalInformationCommand, ISimpleResult> updateSolutionBrowserAdditionalInformationValidator) : base(updateSolutionBrowserAdditionalInformationExecutor, updateSolutionBrowserAdditionalInformationValidator)
        {
        }
    }
}

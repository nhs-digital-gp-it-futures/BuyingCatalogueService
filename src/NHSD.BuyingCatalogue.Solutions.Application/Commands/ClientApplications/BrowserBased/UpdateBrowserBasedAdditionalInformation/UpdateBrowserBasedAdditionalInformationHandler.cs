using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation
{
    internal sealed class UpdateBrowserBasedAdditionalInformationHandler : Handler<UpdateBrowserBasedAdditionalInformationCommand, ISimpleResult>
    {
        public UpdateBrowserBasedAdditionalInformationHandler(IExecutor<UpdateBrowserBasedAdditionalInformationCommand> updateSolutionBrowserAdditionalInformationExecutor,
            IValidator<UpdateBrowserBasedAdditionalInformationCommand, ISimpleResult> updateSolutionBrowserAdditionalInformationValidator) : base(updateSolutionBrowserAdditionalInformationExecutor, updateSolutionBrowserAdditionalInformationValidator)
        {
        }
    }
}

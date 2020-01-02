using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstHandler : Handler<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult>
    {
        public UpdateSolutionBrowserMobileFirstHandler(IExecutor<UpdateSolutionBrowserMobileFirstCommand> updateSolutionBrowserMobileFirstExecutor,
            IValidator<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult> updateSolutionBrowserMobileFirstValidator) : base(updateSolutionBrowserMobileFirstExecutor, updateSolutionBrowserMobileFirstValidator)
        {
        }
    }
}

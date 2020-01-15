using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedHandler : Handler<UpdateSolutionBrowsersSupportedCommand, ISimpleResult>
    {
        public UpdateSolutionBrowsersSupportedHandler(IExecutor<UpdateSolutionBrowsersSupportedCommand> updateSolutionBrowsersSupportedExecutor,
            IValidator<UpdateSolutionBrowsersSupportedCommand, ISimpleResult> updateSolutionBrowsersSupportedValidator) : base(updateSolutionBrowsersSupportedExecutor, updateSolutionBrowsersSupportedValidator)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedHandler : Handler<UpdateSolutionBrowsersSupportedCommand, RequiredResult>
    {
        public UpdateSolutionBrowsersSupportedHandler(IExecutor<UpdateSolutionBrowsersSupportedCommand> updateSolutionBrowsersSupportedExecutor,
            IValidator<UpdateSolutionBrowsersSupportedCommand, RequiredResult> updateSolutionBrowsersSupportedValidator) : base(updateSolutionBrowsersSupportedExecutor, updateSolutionBrowsersSupportedValidator)
        {
        }
    }
}

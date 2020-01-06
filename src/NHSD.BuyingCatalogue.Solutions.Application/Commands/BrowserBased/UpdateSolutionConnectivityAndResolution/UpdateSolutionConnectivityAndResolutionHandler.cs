using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution
{
    internal sealed class UpdateSolutionConnectivityAndResolutionHandler : Handler<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult>
    {
        public UpdateSolutionConnectivityAndResolutionHandler(IExecutor<UpdateSolutionConnectivityAndResolutionCommand> updateSolutionConnectivityAndResolutionExecutor,
            IValidator<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult> updateSolutionConnectivityAndResolutionValidator) : base(updateSolutionConnectivityAndResolutionExecutor, updateSolutionConnectivityAndResolutionValidator)
        {
        }
    }
}

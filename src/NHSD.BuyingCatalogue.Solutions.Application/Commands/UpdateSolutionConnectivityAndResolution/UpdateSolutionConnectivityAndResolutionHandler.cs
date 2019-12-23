using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal sealed class UpdateSolutionConnectivityAndResolutionHandler : Handler<UpdateSolutionConnectivityAndResolutionCommand, RequiredResult>
    {
        public UpdateSolutionConnectivityAndResolutionHandler(IExecutor<UpdateSolutionConnectivityAndResolutionCommand> updateSolutionConnectivityAndResolutionExecutor,
            IValidator<UpdateSolutionConnectivityAndResolutionCommand, RequiredResult> updateSolutionConnectivityAndResolutionValidator) : base(updateSolutionConnectivityAndResolutionExecutor, updateSolutionConnectivityAndResolutionValidator)
        {
        }
    }
}

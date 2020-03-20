using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics
{
    internal sealed class UpdateClaimedEpicsHandler : Handler<UpdateClaimedEpicsCommand, ISimpleResult>
    {
        public UpdateClaimedEpicsHandler(IExecutor<UpdateClaimedEpicsCommand> executor,
            IValidator<UpdateClaimedEpicsCommand, ISimpleResult> validator,
            IVerifier<UpdateClaimedEpicsCommand, ISimpleResult> verifier) :
            base(executor, validator, verifier)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateEpics
{
    internal sealed class UpdateEpicsHandler : Handler<UpdateEpicsCommand, ISimpleResult>
    {
        public UpdateEpicsHandler(IExecutor<UpdateEpicsCommand> executor,
            IValidator<UpdateEpicsCommand, ISimpleResult> validator) :
            base(executor, validator)
        {
        }
    }
}

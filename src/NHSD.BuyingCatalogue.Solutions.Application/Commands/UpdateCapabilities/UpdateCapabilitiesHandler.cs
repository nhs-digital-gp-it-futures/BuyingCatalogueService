using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities
{
    internal sealed class UpdateCapabilitiesHandler : Handler<UpdateCapabilitiesCommand, ISimpleResult>
    {
        public UpdateCapabilitiesHandler(IExecutor<UpdateCapabilitiesCommand> executor,
            IValidator<UpdateCapabilitiesCommand, ISimpleResult> validator) :
            base(executor, validator)
        {
        }
    }
}

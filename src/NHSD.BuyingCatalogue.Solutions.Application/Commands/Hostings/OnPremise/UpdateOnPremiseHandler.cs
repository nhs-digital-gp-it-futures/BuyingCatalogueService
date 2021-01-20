using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise
{
    internal sealed class UpdateOnPremiseHandler : Handler<UpdateOnPremiseCommand, ISimpleResult>
    {
        public UpdateOnPremiseHandler(
            IExecutor<UpdateOnPremiseCommand> executor,
            IValidator<UpdateOnPremiseCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

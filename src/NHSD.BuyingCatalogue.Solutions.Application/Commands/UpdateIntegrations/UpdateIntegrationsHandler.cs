using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations
{
    internal sealed class UpdateIntegrationsHandler : Handler<UpdateIntegrationsCommand, ISimpleResult>
    {
        public UpdateIntegrationsHandler(
            IExecutor<UpdateIntegrationsCommand> executor,
            IValidator<UpdateIntegrationsCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadMap
{
    internal sealed class UpdateRoadMapHandler : Handler<UpdateRoadMapCommand, ISimpleResult>
    {
        public UpdateRoadMapHandler(
            IExecutor<UpdateRoadMapCommand> executor,
            IValidator<UpdateRoadMapCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

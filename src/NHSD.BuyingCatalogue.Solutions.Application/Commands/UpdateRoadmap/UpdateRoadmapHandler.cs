using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap
{
    internal sealed class UpdateRoadmapHandler : Handler<UpdateRoadmapCommand, ISimpleResult>
    {
        public UpdateRoadmapHandler(
            IExecutor<UpdateRoadmapCommand> executor,
            IValidator<UpdateRoadmapCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

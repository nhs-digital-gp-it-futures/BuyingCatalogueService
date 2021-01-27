using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.HybridHostingType
{
    internal sealed class UpdateHybridHostingTypeHandler : Handler<UpdateHybridHostingTypeCommand, ISimpleResult>
    {
        public UpdateHybridHostingTypeHandler(
            IExecutor<UpdateHybridHostingTypeCommand> executor,
            IValidator<UpdateHybridHostingTypeCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    internal sealed class UpdateSolutionSupplierHandler : Handler<UpdateSolutionSupplierCommand, ISimpleResult>
    {
        public UpdateSolutionSupplierHandler(
            IExecutor<UpdateSolutionSupplierCommand> executor,
            IValidator<UpdateSolutionSupplierCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

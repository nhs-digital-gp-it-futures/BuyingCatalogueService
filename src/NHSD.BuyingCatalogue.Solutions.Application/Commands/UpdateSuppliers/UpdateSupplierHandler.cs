using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    internal sealed class UpdateSupplierHandler : Handler<UpdateSupplierCommand, ISimpleResult>
    {
        public UpdateSupplierHandler(IExecutor<UpdateSupplierCommand> executor,
            IValidator<UpdateSupplierCommand, ISimpleResult> validator) :
            base(executor, validator)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales
{
    internal sealed class UpdateImplementationTimescalesHandler : Handler<UpdateImplementationTimescalesCommand, ISimpleResult>
    {
        public UpdateImplementationTimescalesHandler(
            IExecutor<UpdateImplementationTimescalesCommand> executor,
            IValidator<UpdateImplementationTimescalesCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

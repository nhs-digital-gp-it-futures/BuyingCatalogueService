using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsHandler : Handler<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult>
    {
        public UpdateSolutionMobileConnectionDetailsHandler(IExecutor<UpdateSolutionMobileConnectionDetailsCommand> updateSolutionMobileConnectionDetailsExecutor,
            IValidator<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult> updateSolutionMobileConnectionDetailsValidator) : base(updateSolutionMobileConnectionDetailsExecutor, updateSolutionMobileConnectionDetailsValidator)
        {
        }
    }
}

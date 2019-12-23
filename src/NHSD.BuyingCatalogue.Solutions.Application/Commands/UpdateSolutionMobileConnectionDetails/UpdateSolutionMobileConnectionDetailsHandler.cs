using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsHandler : Handler<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult>
    {
        public UpdateSolutionMobileConnectionDetailsHandler(IExecutor<UpdateSolutionMobileConnectionDetailsCommand> updateSolutionMobileConnectionDetailsExecutor,
            IValidator<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult> updateSolutionMobileConnectionDetailsValidator) : base(updateSolutionMobileConnectionDetailsExecutor, updateSolutionMobileConnectionDetailsValidator)
        {
        }
    }
}

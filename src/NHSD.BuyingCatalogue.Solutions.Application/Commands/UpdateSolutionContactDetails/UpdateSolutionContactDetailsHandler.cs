using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal sealed class UpdateSolutionContactDetailsHandler : Handler<UpdateSolutionContactDetailsCommand, MaxLengthResult>
    {
        public UpdateSolutionContactDetailsHandler(IExecutor<UpdateSolutionContactDetailsCommand> updateSolutionContactDetailsExecutor,
            IValidator<UpdateSolutionContactDetailsCommand, MaxLengthResult> updateSolutionContactDetailsValidator) : base(updateSolutionContactDetailsExecutor, updateSolutionContactDetailsValidator)
        {
        }
    }
}

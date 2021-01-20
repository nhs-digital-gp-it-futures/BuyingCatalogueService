using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal sealed class UpdateSolutionContactDetailsHandler : Handler<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult>
    {
        public UpdateSolutionContactDetailsHandler(
            IExecutor<UpdateSolutionContactDetailsCommand> updateSolutionContactDetailsExecutor,
            IValidator<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult> updateSolutionContactDetailsValidator)
            : base(updateSolutionContactDetailsExecutor, updateSolutionContactDetailsValidator)
        {
        }
    }
}

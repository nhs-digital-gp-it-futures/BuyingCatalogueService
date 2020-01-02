using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryHandler : Handler<UpdateSolutionSummaryCommand, ISimpleResult>
    {
        public UpdateSolutionSummaryHandler(IExecutor<UpdateSolutionSummaryCommand> updateSolutionSummaryExecutor,
            IValidator<UpdateSolutionSummaryCommand, ISimpleResult> updateSolutionSummaryValidator) : base(updateSolutionSummaryExecutor, updateSolutionSummaryValidator)
        {
        }
    }
}

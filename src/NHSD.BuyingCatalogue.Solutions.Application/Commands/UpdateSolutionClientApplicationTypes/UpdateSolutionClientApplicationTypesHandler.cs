using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesHandler : Handler<UpdateSolutionClientApplicationTypesCommand, RequiredResult>
    {
        public UpdateSolutionClientApplicationTypesHandler(IExecutor<UpdateSolutionClientApplicationTypesCommand> updateSolutionClientApplicationTypesExecutor,
            IValidator<UpdateSolutionClientApplicationTypesCommand, RequiredResult> updateSolutionClientApplicationTypesValidator) : base(updateSolutionClientApplicationTypesExecutor, updateSolutionClientApplicationTypesValidator)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileMemoryAndStorage
{
    internal sealed class UpdateSolutionMobileMemoryStorageHandler : Handler<UpdateSolutionMobileMemoryStorageCommand, RequiredMaxLengthResult>
    {
        public UpdateSolutionMobileMemoryStorageHandler(IExecutor<UpdateSolutionMobileMemoryStorageCommand> updateSolutionMobileMemoryStorageExecutor,
            IValidator<UpdateSolutionMobileMemoryStorageCommand, RequiredMaxLengthResult> updateSolutionMobileMemoryStorageValidator) : base(updateSolutionMobileMemoryStorageExecutor, updateSolutionMobileMemoryStorageValidator)
        {
        }
    }
}

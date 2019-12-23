using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsHandler : Handler<UpdateSolutionPluginsCommand, RequiredMaxLengthResult>
    {
        public UpdateSolutionPluginsHandler(IExecutor<UpdateSolutionPluginsCommand> updateSolutionPluginsExecutor,
            IValidator<UpdateSolutionPluginsCommand, RequiredMaxLengthResult> updateSolutionPluginsValidator) : base(updateSolutionPluginsExecutor, updateSolutionPluginsValidator)
        {
        }
    }
}

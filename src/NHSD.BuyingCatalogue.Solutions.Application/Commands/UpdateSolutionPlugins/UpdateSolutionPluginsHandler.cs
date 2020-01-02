using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsHandler : Handler<UpdateSolutionPluginsCommand, ISimpleResult>
    {
        public UpdateSolutionPluginsHandler(IExecutor<UpdateSolutionPluginsCommand> updateSolutionPluginsExecutor,
            IValidator<UpdateSolutionPluginsCommand, ISimpleResult> updateSolutionPluginsValidator) : base(updateSolutionPluginsExecutor, updateSolutionPluginsValidator)
        {
        }
    }
}

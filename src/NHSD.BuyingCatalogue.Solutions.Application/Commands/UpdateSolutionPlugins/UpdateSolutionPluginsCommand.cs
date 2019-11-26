using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    public sealed class UpdateSolutionPluginsCommand : IRequest<UpdateSolutionPluginsValidationResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionPluginsViewModel UpdateSolutionPluginsViewModel { get; }

        public UpdateSolutionPluginsCommand(string solutionId, UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionPluginsViewModel = updateSolutionPluginsViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionPluginsViewModel));
        }
    }
}

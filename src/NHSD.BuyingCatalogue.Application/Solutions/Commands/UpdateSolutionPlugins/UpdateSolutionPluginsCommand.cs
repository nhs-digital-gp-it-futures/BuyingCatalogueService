using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins
{
    public sealed class UpdateSolutionPluginsCommand : IRequest<UpdateSolutionPluginsValidationResult>
    {
        public string SolutionId { get; set; }

        public UpdateSolutionPluginsViewModel UpdateSolutionPluginsViewModel { get; }

        public UpdateSolutionPluginsCommand(string solutionId, UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionPluginsViewModel = updateSolutionPluginsViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionPluginsViewModel));
        }
    }
}

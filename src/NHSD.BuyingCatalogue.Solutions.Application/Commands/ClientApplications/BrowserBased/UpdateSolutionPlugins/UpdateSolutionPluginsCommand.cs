using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins
{
    public sealed class UpdateSolutionPluginsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionPluginsViewModel Data { get; }

        public UpdateSolutionPluginsCommand(string solutionId, UpdateSolutionPluginsViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

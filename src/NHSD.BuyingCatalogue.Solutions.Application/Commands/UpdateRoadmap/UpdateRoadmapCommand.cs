using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap
{
    public sealed class UpdateRoadmapCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string Summary { get; }

        public UpdateRoadmapCommand(string solutionId, string summary)
        {
            SolutionId = solutionId.ThrowIfNull();
            Summary = summary;
        }
    }
}

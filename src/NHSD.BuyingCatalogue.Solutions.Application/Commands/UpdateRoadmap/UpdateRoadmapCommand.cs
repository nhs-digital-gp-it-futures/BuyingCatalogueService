using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap
{
    public sealed class UpdateRoadmapCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string Description { get; }

        public UpdateRoadmapCommand(string solutionId, string description)
        {
            SolutionId = solutionId.ThrowIfNull();
            Description = description;
        }
    }
}

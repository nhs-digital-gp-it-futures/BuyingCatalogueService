using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap
{
    public sealed class UpdateRoadmapCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string Summary { get; }

        public UpdateRoadmapCommand(string solutionId, string summary)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Summary = summary;
        }
    }
}

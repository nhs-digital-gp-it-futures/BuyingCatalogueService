using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadMap
{
    public sealed class UpdateRoadMapCommand : IRequest<ISimpleResult>
    {
        public UpdateRoadMapCommand(string solutionId, string summary)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Summary = summary;
        }

        public string SolutionId { get; }

        public string Summary { get; }
    }
}

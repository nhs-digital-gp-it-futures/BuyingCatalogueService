using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails
{
    public sealed class UpdateSolutionMobileConnectionDetailsCommand : IRequest<MaxLengthResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionMobileConnectionDetailsViewModel Details { get; }

        public UpdateSolutionMobileConnectionDetailsCommand(string solutionId, UpdateSolutionMobileConnectionDetailsViewModel details)
        {
            SolutionId = solutionId;
            Details = details;
        }
    }
}

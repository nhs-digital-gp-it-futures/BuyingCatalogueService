using System.Collections.Generic;
using System.Linq;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    public sealed class UpdateSolutionMobileConnectionDetailsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionMobileConnectionDetailsViewModel Data { get; }

        public UpdateSolutionMobileConnectionDetailsCommand(string solutionId, UpdateSolutionMobileConnectionDetailsViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

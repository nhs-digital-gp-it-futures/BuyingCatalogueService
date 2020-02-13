using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics.UpdateEpics
{
    public sealed class UpdateClaimedEpicsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public HashSet<IClaimedEpic> Data { get; }

        public UpdateClaimedEpicsCommand(string solutionId, HashSet<IClaimedEpic> data)
        {
            SolutionId = solutionId.ThrowIfNull(nameof(solutionId));

            Data = data ?? new HashSet<IClaimedEpic>();
        }
    }
}

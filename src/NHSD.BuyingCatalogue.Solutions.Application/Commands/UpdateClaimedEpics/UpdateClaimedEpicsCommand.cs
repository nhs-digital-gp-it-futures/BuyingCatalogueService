using System;
using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics
{
    public sealed class UpdateClaimedEpicsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IEnumerable<IClaimedEpic> Data { get; }

        public UpdateClaimedEpicsCommand(string solutionId, IEnumerable<IClaimedEpic> data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? new HashSet<IClaimedEpic>();
        }
    }
}

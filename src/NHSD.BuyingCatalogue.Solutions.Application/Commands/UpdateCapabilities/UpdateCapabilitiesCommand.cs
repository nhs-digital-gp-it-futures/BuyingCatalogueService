using System;
using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities
{
    public sealed class UpdateCapabilitiesCommand : IRequest<ISimpleResult>
    {
        public UpdateCapabilitiesCommand(string solutionId, HashSet<string> newCapabilitiesReferences)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            NewCapabilitiesReferences = newCapabilitiesReferences;
        }

        public string SolutionId { get; }

        public HashSet<string> NewCapabilitiesReferences { get; }
    }
}

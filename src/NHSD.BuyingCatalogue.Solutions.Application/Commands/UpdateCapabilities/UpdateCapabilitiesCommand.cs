using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities
{
    public sealed class UpdateCapabilitiesCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public HashSet<string> NewCapabilitiesReferences { get; }

        public UpdateCapabilitiesCommand(string solutionId, HashSet<string> newCapabilitiesReferences)
        {
            SolutionId = solutionId.ThrowIfNull(nameof(solutionId));
            NewCapabilitiesReferences = newCapabilitiesReferences;
        }
    }
}

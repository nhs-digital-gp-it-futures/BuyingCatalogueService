using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateCapabilityRequest : IUpdateCapabilityRequest
    {
        public string SolutionId { get; }
        public IEnumerable<string> NewCapabilitiesReference { get; }

        public UpdateCapabilityRequest(string solutionId, IEnumerable<string> newCapabilitiesReference)
        {
            SolutionId = solutionId;
            NewCapabilitiesReference = newCapabilitiesReference;
        }
    }
}

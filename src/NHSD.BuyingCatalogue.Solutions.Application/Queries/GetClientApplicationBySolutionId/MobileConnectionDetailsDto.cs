using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class MobileConnectionDetailsDto : IMobileConnectionDetails
    {
        public HashSet<string> ConnectionType { get; internal set; } = new HashSet<string>();
        public string MinimumConnectionSpeed { get; set; }
        public string Description { get; set; }
    }
}

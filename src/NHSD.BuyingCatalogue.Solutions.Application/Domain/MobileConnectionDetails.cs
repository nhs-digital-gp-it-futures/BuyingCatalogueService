using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class MobileConnectionDetails
    {
        public string MinimumConnectionSpeed { get; set; }
        public HashSet<string> ConnectionType { get; set; } = new HashSet<string>();
        public string Description { get; set; }
    }
}

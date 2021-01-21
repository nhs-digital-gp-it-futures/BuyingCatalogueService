using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IMobileConnectionDetails
    {
        HashSet<string> ConnectionType { get; }

        string MinimumConnectionSpeed { get; }

        string Description { get; }
    }
}

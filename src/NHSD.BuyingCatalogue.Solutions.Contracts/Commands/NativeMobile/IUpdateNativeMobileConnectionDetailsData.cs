using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile
{
    public interface IUpdateNativeMobileConnectionDetailsData
    {
        string MinimumConnectionSpeed { get; }

        HashSet<string> ConnectionType { get; }

        string ConnectionRequirementsDescription { get; }
    }
}

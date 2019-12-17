using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IMobileOperatingSystems
    {
        HashSet<string> OperatingSystems { get; }
        string OperatingSystemsDescription { get; }
    }
}

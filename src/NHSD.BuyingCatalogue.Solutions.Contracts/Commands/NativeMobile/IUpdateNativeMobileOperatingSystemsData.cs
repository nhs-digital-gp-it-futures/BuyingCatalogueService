using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile
{
    public interface IUpdateNativeMobileOperatingSystemsData
    {
        HashSet<string> OperatingSystems { get; }

        string OperatingSystemsDescription { get; }
    }
}

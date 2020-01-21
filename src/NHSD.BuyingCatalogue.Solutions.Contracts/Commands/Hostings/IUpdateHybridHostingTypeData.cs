using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IUpdateHybridHostingTypeData
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        HashSet<string> RequiresHSCN { get; }
    }
}

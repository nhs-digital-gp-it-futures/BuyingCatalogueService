using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IUpdatePrivateCloudData
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHSCN { get; }
    }
}

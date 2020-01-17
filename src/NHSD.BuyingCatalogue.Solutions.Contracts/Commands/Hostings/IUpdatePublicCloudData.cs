using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IUpdatePublicCloudData
    {
        string Summary { get; }

        string URL { get; }

        HashSet<string> ConnectivityRequired { get; }
    }
}

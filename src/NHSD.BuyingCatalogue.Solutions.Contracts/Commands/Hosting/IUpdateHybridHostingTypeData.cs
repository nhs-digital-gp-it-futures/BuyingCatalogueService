﻿namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting
{
    public interface IUpdateHybridHostingTypeData
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHscn { get; }
    }
}

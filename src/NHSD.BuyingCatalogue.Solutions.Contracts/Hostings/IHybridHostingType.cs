namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IHybridHostingType
    {
        string Summary { get; set; }

        string Link { get; set; }

        string HostingModel { get; set; }

        string RequiresHSCN { get; set;  }
    }
}

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IHybridHostingType
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        public string HostingModel { get; set; }

        public string RequiresHSCN { get; set;  }
    }
}

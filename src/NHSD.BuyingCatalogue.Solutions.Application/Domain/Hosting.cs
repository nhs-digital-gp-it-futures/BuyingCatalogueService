using NHSD.BuyingCatalogue.Solutions.Application.Domain.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class Hosting
    {
        public PublicCloud PublicCloud { get; set; }

        public PrivateCloud PrivateCloud { get; set; }

        public HybridHostingType HybridHostingType { get; set; }
        
        public OnPremise OnPremise { get; set; }
    }
}

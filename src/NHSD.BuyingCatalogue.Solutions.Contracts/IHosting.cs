using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IHosting
    {
        IPublicCloud PublicCloud { get; }

        IPrivateCloud PrivateCloud { get; }

        IHybridHostingType HybridHostingType { get; }

        IOnPremise OnPremise { get; }
    }
}

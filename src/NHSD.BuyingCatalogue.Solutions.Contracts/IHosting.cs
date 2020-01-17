using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IHosting
    {
        IPublicCloud PublicCloud { get; }
        
		IPrivateCloud PrivateCloud { get; }

		IHostingTypeHybrid HostingTypeHybrid { get; }
    }
}

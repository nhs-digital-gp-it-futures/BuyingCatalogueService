using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId
{
    internal sealed class HostingDto : IHosting
    {
        public IPublicCloud PublicCloud { get; set; }
        
        public IPrivateCloud PrivateCloud { get; set; }
    	
		public IHybridHostingType HybridHostingType { get; set; }
		
		public IOnPremise OnPremise { get; set; }
    }
}

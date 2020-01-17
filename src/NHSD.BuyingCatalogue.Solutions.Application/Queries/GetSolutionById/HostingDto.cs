using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal sealed class HostingDto : IHosting
    {
        public IPublicCloud PublicCloud { get; set; }
        
    	public IPrivateCloud PrivateCloud { get; set; }
    	
		public IHostingTypeHybrid HostingTypeHybrid { get; set; }
    }
}

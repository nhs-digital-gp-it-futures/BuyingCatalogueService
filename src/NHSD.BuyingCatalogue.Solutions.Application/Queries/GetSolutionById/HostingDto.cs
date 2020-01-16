using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal sealed class HostingDto : IHosting
    {
    	public IPublicCloud PublicCloud { get; set; }
    	
		public IHostingTypeHybrid HostingTypeHybrid { get; set; }
    }
}

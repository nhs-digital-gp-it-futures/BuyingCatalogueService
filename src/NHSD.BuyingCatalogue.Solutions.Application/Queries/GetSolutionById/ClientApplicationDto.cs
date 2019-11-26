using System.Collections.Generic;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal class ClientApplicationDto : IClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }

        public IPlugins Plugins { get; set; }
    }
}

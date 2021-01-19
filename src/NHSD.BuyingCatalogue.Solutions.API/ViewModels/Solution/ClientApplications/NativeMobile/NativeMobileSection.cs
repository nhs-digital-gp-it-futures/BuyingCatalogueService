using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class NativeMobileSection
    {
        public NativeMobileSection(IClientApplication clientApplication)
        {
            Sections = new NativeMobileSubSections(clientApplication);
        }

        [JsonProperty("sections")]
        public NativeMobileSubSections Sections { get; }

        public NativeMobileSection IfPopulated() => Sections.HasData ? this : null;
    }
}

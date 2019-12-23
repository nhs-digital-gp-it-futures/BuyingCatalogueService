using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class NativeMobileSection
    {
        [JsonProperty("sections")]
        public NativeMobileSubSections Sections { get; }

        public NativeMobileSection(IClientApplication clientApplication)
        {
            Sections = new NativeMobileSubSections(clientApplication);
        }

        public NativeMobileSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}

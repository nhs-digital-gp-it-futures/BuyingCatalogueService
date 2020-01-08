using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopSection
    {
        [JsonProperty("sections")]
        public NativeDesktopSubSections Sections { get; }

        public NativeDesktopSection(IClientApplication clientApplication)
        {
            Sections = new NativeDesktopSubSections(clientApplication);
        }

        public NativeDesktopSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}

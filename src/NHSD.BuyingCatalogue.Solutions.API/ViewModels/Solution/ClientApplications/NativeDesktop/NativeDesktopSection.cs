using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopSection
    {
        public NativeDesktopSection(IClientApplication clientApplication)
        {
            Sections = new NativeDesktopSubSections(clientApplication);
        }

        [JsonProperty("sections")]
        public NativeDesktopSubSections Sections { get; }

        public NativeDesktopSection IfPopulated() => Sections.HasData ? this : null;
    }
}

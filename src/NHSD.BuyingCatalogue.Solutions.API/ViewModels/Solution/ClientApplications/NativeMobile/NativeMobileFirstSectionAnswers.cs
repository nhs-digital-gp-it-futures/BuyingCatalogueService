using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class NativeMobileFirstSectionAnswers
    {
        public NativeMobileFirstSectionAnswers(IClientApplication clientApplication) =>
            MobileFirstDesign = clientApplication?.NativeMobileFirstDesign.ToYesNoString();

        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(MobileFirstDesign);
    }
}

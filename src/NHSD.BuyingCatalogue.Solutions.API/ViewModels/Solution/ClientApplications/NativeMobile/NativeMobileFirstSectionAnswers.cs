using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class NativeMobileFirstSectionAnswers
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(MobileFirstDesign);

        public NativeMobileFirstSectionAnswers(IClientApplication clientApplication) =>
            MobileFirstDesign = clientApplication?.NativeMobileFirstDesign.ToYesNoString();
    }
}

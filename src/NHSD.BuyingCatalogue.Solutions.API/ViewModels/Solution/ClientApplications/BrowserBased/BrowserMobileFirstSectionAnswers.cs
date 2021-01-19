using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserMobileFirstSectionAnswers
    {
        public BrowserMobileFirstSectionAnswers(IClientApplication clientApplication) =>
            MobileFirstDesign = clientApplication?.MobileFirstDesign.ToYesNoString();

        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; }

        [JsonIgnore]
        public bool HasData => MobileFirstDesign?.Any() == true;
    }
}

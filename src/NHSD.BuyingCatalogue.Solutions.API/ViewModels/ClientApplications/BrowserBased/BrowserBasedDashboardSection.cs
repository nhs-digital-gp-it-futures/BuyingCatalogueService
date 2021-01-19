using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class BrowserBasedDashboardSection
    {
        private readonly bool mandatory;
        private readonly bool complete;

        public BrowserBasedDashboardSection(bool complete, bool mandatory)
        {
            this.complete = complete;
            this.mandatory = mandatory;
        }

        [JsonProperty("status")]
        public string Status => complete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("requirement")]
        public string Requirement => mandatory ? "Mandatory" : "Optional";
    }
}

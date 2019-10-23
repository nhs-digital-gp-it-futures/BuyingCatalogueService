using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class BrowserBasedResult
    {
        [JsonProperty("sections")]
        public List<BrowserBasedResultSection> Sections { get; set; }
    }

    public class BrowserBasedResultSection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("requirement")]
        public string Requirement { get; set; }
    }
}

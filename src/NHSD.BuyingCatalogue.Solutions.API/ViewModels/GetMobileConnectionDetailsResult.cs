using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class GetMobileConnectionDetailsResult
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; set; }

        [JsonProperty("connection-type")]
        public IEnumerable<string> ConnectionType { get; set; }

        [JsonProperty("connection-requirements-description")]
        public string ConnectionRequirementsDescription { get; set; }
    }
}

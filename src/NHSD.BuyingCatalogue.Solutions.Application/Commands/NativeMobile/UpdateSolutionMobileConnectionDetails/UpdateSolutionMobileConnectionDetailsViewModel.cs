using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    public class UpdateSolutionMobileConnectionDetailsViewModel
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; set; }

        [JsonProperty("connection-types")]
        public HashSet<string> ConnectionType { get; internal set; }

        [JsonProperty("connection-requirements-description")]
        public string ConnectionRequirementsDescription { get; set; }
    }
}

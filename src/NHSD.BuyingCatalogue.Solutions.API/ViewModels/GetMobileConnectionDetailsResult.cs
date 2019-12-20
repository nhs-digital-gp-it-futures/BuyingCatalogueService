using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetMobileConnectionDetailsResult
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; private set; }

        [JsonProperty("connection-types")]
        public IEnumerable<string> ConnectionType { get; private set; }

        [JsonProperty("connection-requirements-description")]
        public string ConnectionRequirementsDescription { get; private set; }

        public GetMobileConnectionDetailsResult(IMobileConnectionDetails connectionDetails)
        {
            ConnectionType = connectionDetails?.ConnectionType ?? new HashSet<string>();
            MinimumConnectionSpeed = connectionDetails?.MinimumConnectionSpeed;
            ConnectionRequirementsDescription = connectionDetails?.Description;
        }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class GetMobileConnectionDetailsResult
    {
        public GetMobileConnectionDetailsResult(IMobileConnectionDetails connectionDetails)
        {
            ConnectionType = connectionDetails?.ConnectionType ?? new HashSet<string>();
            MinimumConnectionSpeed = connectionDetails?.MinimumConnectionSpeed;
            ConnectionRequirementsDescription = connectionDetails?.Description;
        }

        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; }

        [JsonProperty("connection-types")]
        public IEnumerable<string> ConnectionType { get; }

        [JsonProperty("connection-requirements-description")]
        public string ConnectionRequirementsDescription { get; }
    }
}

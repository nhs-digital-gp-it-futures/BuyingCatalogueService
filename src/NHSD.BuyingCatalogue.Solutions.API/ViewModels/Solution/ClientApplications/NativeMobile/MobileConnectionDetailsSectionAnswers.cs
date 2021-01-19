using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileConnectionDetailsSectionAnswers
    {
        public MobileConnectionDetailsSectionAnswers(IMobileConnectionDetails connectionDetails)
        {
            ConnectionType = connectionDetails?.ConnectionType?.Any() == true
                ? connectionDetails.ConnectionType
                : null;

            MinimumConnectionSpeed = connectionDetails?.MinimumConnectionSpeed;
            Description = connectionDetails?.Description;
        }

        [JsonProperty("connection-types")]
        public IEnumerable<string> ConnectionType { get; }

        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; }

        [JsonProperty("connection-requirements-description")]
        public string Description { get; }

        [JsonIgnore]
        public bool HasData => ConnectionType?.Any() == true
            || !string.IsNullOrWhiteSpace(MinimumConnectionSpeed)
            || !string.IsNullOrWhiteSpace(Description);
    }
}

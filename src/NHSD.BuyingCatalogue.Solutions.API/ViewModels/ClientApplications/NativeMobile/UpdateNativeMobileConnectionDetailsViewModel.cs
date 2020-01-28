using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class UpdateNativeMobileConnectionDetailsViewModel : IUpdateNativeMobileConnectionDetailsData
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; set; }

        [JsonProperty("connection-types")]
        public HashSet<string> ConnectionType { get; internal set; }

        [JsonProperty("connection-requirements-description")]
        public string ConnectionRequirementsDescription { get; set; }
    }
}

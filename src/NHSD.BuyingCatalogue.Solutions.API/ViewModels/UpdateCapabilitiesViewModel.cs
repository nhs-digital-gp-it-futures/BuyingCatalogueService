using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateCapabilitiesViewModel
    {
        [JsonProperty("capabilities-reference")]
        public HashSet<string> NewCapabilitiesReferences { get; internal set; } = new HashSet<string>();
    }
}

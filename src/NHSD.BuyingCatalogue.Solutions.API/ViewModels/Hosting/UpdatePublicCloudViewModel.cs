using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting
{
    public sealed class UpdatePublicCloudViewModel : IUpdatePublicCloudData
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiresHscnArray { get; internal set; } = new();

        [JsonIgnore]
        public string RequiresHscn
        {
            get
            {
                return RequiresHscnArray.FirstOrDefault();
            }
        }
    }
}

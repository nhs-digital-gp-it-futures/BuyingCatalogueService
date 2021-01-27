using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting
{
    public sealed class UpdateOnPremiseViewModel : IUpdateOnPremiseData
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiresHSCNArray { get; internal set; } = new();

        [JsonIgnore]
        public string RequiresHSCN
        {
            get
            {
                return RequiresHSCNArray.FirstOrDefault();
            }
        }
    }
}

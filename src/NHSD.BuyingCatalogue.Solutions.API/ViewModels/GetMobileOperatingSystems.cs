using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class GetMobileOperatingSystems
    {
        [JsonProperty("operating-systems")]
        public IEnumerable<string> OperatingSystems { get; set; } 

        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }
    }
}

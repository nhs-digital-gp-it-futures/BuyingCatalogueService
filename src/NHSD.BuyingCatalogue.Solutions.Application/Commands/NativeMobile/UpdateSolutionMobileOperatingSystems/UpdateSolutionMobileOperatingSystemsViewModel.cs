using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    public class UpdateSolutionMobileOperatingSystemsViewModel
    {
        [JsonProperty("operating-systems")]
        public HashSet<string> OperatingSystems { get; internal set; } 

        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }
    }
}

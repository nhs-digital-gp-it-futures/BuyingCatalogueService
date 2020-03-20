using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public class GetMobileOperatingSystemsResult
    {
        [JsonProperty("operating-systems")]
        public IEnumerable<string> OperatingSystems { get; set; }

        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }

        public GetMobileOperatingSystemsResult(IMobileOperatingSystems operatingSystems)
        {
            OperatingSystems = operatingSystems?.OperatingSystems ?? new HashSet<string>();
            OperatingSystemsDescription = operatingSystems?.OperatingSystemsDescription;
        }
    }
}

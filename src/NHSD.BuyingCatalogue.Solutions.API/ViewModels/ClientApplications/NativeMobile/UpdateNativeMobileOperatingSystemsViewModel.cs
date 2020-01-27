using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class UpdateNativeMobileOperatingSystemsViewModel : IUpdateNativeMobileOperatingSystemsData
    {
        [JsonProperty("operating-systems")]
        public HashSet<string> OperatingSystems { get; internal set; }

        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }
    }
}

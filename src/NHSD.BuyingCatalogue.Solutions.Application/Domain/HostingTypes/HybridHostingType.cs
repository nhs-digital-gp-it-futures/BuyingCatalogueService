﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.HostingTypes
{
    internal sealed class HybridHostingType : IHybridHostingType
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        public string HostingModel { get; set; }

        [JsonProperty("RequiresHSCN")]
        public string RequiresHscn { get; set; }
    }
}

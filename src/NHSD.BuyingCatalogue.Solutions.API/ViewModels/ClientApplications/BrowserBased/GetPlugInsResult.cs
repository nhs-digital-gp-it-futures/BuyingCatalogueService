﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class GetPlugInsResult
    {
        public GetPlugInsResult(IPlugins plugins)
        {
            PlugIns = plugins?.Required.ToYesNoString();
            AdditionalInformation = plugins?.AdditionalInformation;
        }

        [JsonProperty("plugins-required")]
        public string PlugIns { get; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get; }
    }
}

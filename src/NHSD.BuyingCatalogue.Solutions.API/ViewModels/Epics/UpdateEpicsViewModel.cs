using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics
{
    public sealed class UpdateEpicsViewModel
    {
        [JsonProperty("epics")]
        public HashSet<ClaimedEpicViewModel> ClaimedEpics { get; internal set; } = new();
    }
}

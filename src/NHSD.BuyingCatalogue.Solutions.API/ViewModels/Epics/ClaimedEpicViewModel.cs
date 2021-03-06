using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics
{
    public sealed class ClaimedEpicViewModel : IClaimedEpic
    {
        [JsonProperty("epic-id")]
        public string EpicId { get; set; }

        [JsonProperty("assessment-result")]
        public string StatusName { get; set; }
    }
}

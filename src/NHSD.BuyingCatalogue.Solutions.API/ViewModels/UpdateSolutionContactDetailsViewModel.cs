using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionContactDetailsViewModel : IUpdateSolutionContactDetails
    {
        [JsonProperty("contact-1")]
        [JsonConverter(typeof(UpdateSolutionContactViewModelConverter))]
        public IUpdateSolutionContact Contact1 { get; set; }

        [JsonProperty("contact-2")]
        [JsonConverter(typeof(UpdateSolutionContactViewModelConverter))]
        public IUpdateSolutionContact Contact2 { get; set; }
    }
}

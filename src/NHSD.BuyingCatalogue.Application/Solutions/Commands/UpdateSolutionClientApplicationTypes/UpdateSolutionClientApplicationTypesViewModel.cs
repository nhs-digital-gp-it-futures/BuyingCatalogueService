using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes
{
    public sealed class UpdateSolutionClientApplicationTypesViewModel
    {
        private static readonly HashSet<string> _acceptedClientApplicationTypes = new HashSet<string> { "browser-based", "native-mobile", "native-desktop" };

        [JsonProperty("client-application-types")]
        public HashSet<string> ClientApplicationTypes { get; set; }

        [JsonIgnore]
        public List<string> FilteredClientApplicationTypes =>
            ClientApplicationTypes.Where(s => _acceptedClientApplicationTypes.Contains(s)).ToList();
    }
}

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    public sealed class UpdateSolutionClientApplicationTypesViewModel
    {
        private static readonly HashSet<string> AcceptedClientApplicationTypes = new HashSet<string> { "browser-based", "native-mobile", "native-desktop" };

        [JsonProperty("client-application-types")]
        public HashSet<string> ClientApplicationTypes { get; internal set; }

        [JsonIgnore]
        public IEnumerable<string> FilteredClientApplicationTypes =>
            ClientApplicationTypes.Where(s => AcceptedClientApplicationTypes.Contains(s));
    }
}

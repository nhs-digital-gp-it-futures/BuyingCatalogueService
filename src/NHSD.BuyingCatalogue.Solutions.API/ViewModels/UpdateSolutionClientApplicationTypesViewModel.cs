using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionClientApplicationTypesViewModel : IUpdateSolutionClientApplicationTypes
    {
        private static readonly HashSet<string> AcceptedClientApplicationTypes = new HashSet<string>
        {
            "browser-based",
            "native-mobile",
            "native-desktop"
        };

        public UpdateSolutionClientApplicationTypesViewModel(HashSet<string> clientApplicationTypes) =>
            ClientApplicationTypes = clientApplicationTypes;

        [JsonProperty("client-application-types")]
        public HashSet<string> ClientApplicationTypes { get; }

        [JsonIgnore]
        public IEnumerable<string> FilteredClientApplicationTypes =>
            ClientApplicationTypes.Where(s => AcceptedClientApplicationTypes.Contains(s));
    }
}

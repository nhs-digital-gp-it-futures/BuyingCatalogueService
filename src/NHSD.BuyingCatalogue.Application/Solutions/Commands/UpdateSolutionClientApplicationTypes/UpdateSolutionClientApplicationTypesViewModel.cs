using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionClientApplicationTypesViewModel
    {
        [JsonProperty("client-application-types")]
        public HashSet<string> ClientApplicationTypes { get; set; }
    }
}

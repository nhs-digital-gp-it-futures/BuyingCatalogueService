using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications
{
    public sealed class GetClientApplicationTypesResult
    {
        public GetClientApplicationTypesResult(IClientApplication clientApplication)
        {
            ClientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();
        }

        [JsonProperty("client-application-types")]
        public IEnumerable<string> ClientApplicationTypes { get; }
    }
}

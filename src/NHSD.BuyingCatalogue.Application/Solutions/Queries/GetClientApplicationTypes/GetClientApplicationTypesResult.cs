using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes
{
    public sealed class GetClientApplicationTypesResult
    {
        public GetClientApplicationTypesResult(ClientApplication clientApplication)
        {
            ClientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();
        }

        [JsonProperty("client-application-types")]
        public IEnumerable<string> ClientApplicationTypes { get; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class GetClientApplicationTypesResult
    {
        [JsonProperty("client-application-types")]
        public IEnumerable<string> ClientApplicationTypes { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetClientApplicationTypesResult"/> class.
        /// </summary>
        public GetClientApplicationTypesResult(ClientApplication clientApplication)
        {
            ClientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();
        }
    }
}

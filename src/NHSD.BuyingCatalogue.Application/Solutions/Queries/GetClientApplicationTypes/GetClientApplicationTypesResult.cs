using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes
{
    public sealed class GetClientApplicationTypesResult
    {
        public GetClientApplicationTypesResult(ClientApplicationTypes clientApplicationTypes)
        {
            var clientApplicationTypesSelected = new List<string>();

            if (clientApplicationTypes?.BrowserBased != null)
            {
                clientApplicationTypesSelected.Add("browser-based");
            }

            if (clientApplicationTypes?.NativeMobile != null)
            {
                clientApplicationTypesSelected.Add("native-mobile");
            }

            if (clientApplicationTypes?.NativeDesktop != null)
            {
                clientApplicationTypesSelected.Add("native-desktop");
            }

            ClientApplicationTypes = clientApplicationTypesSelected;
        }

        [JsonProperty("client-application-types")]
        public IEnumerable<string> ClientApplicationTypes { get; }
    }
}

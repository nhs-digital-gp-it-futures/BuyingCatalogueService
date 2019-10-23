using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class Features
    {
        internal Features(string featuresJson)
        {
            Listing = string.IsNullOrWhiteSpace(featuresJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(featuresJson);
        }

        public IEnumerable<string> Listing { get; }
    }
}

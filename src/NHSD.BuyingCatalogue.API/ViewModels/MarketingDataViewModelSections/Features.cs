using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class Features
    {
        internal Features(IEnumerable<string> features)
        {
            Listing = features;
        }

        public IEnumerable<string> Listing { get; }
    }
}

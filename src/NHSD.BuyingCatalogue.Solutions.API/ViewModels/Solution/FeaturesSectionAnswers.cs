using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class FeaturesSectionAnswers
    {
        public FeaturesSectionAnswers(IEnumerable<string> features)
        {
            Listing = features;
        }

        public IEnumerable<string> Listing { get; }

        [JsonIgnore]
        public bool HasData => Listing?.Any(s => !string.IsNullOrWhiteSpace(s)) == true;
    }
}

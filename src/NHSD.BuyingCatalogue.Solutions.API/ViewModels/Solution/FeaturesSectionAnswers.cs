using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class FeaturesSectionAnswers
    {
        public IEnumerable<string> Listing { get; }

        [JsonIgnore]
        public bool HasData => Listing?.Any(x=> !String.IsNullOrWhiteSpace(x)) == true;

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesSectionAnswers"/> class.
        /// </summary>
        public FeaturesSectionAnswers(IEnumerable<string> features)
        {
            Listing = features;
        }
    }
}

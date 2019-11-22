using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solution.API.ViewModels.Preview
{
    public class FeaturesPreviewSectionAnswers
    {
        public IEnumerable<string> Listing { get; }

        [JsonIgnore]
        public bool HasData => Listing?.Any(x => !String.IsNullOrWhiteSpace(x)) == true;

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesPreviewSectionAnswers"/> class.
        /// </summary>
        public FeaturesPreviewSectionAnswers(IEnumerable<string> features)
        {
            Listing = features;
        }
    }
}

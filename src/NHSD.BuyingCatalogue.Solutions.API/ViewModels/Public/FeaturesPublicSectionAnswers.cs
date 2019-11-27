using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class FeaturesPublicSectionAnswers
    {
        public IEnumerable<string> Listing { get; }

        [JsonIgnore]
        public bool HasData => Listing?.Any(x=> !String.IsNullOrWhiteSpace(x)) == true;

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesPublicSectionAnswers"/> class.
        /// </summary>
        public FeaturesPublicSectionAnswers(IEnumerable<string> features)
        {
            Listing = features;
        }
    }
}

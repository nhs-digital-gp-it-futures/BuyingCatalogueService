using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class FeaturesResult
    {
        public FeaturesResult(Solution solution)
        {
            Listing = solution.Features;
        }

        public IEnumerable<string> Listing { get; }
    }
}

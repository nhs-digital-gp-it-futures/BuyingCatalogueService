using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class FeaturesResult
    {
        public FeaturesResult(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Listing = solution.Features;
        }

        public IEnumerable<string> Listing { get; }
    }
}

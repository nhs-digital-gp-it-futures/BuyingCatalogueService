using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class FeaturesResult
    {
        public IEnumerable<string> Listing { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesResult"/> class.
        /// </summary>
        public FeaturesResult(Solution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Listing = solution.Features;
        }
    }
}

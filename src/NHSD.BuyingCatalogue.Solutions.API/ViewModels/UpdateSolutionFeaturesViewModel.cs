﻿using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionFeaturesViewModel : IUpdateSolutionFeatures
    {
        /// <summary>
        /// Gets or sets the features of the solution.
        /// </summary>
        public IEnumerable<string> Listing { get; set; }
    }
}

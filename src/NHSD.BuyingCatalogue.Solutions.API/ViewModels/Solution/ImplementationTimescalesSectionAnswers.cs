﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class ImplementationTimescalesSectionAnswers
    {
        public ImplementationTimescalesSectionAnswers(IImplementationTimescales implementationTimescales)
        {
            Description = implementationTimescales?.Description;
        }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Description);
    }
}

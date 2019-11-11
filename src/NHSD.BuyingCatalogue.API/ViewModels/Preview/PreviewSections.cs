using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels.Preview
{
    public class PreviewSections
    {
        [JsonProperty("solution-description")]
        public SolutionDescriptionPreviewSection SolutionDescription { get; }

        public FeaturesPreviewSection Features { get; }

        [JsonProperty("client-application-types")]
        public ClientApplicationTypesPreviewSection ClientApplicationTypes { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PreviewSections"/> class.
        /// </summary>
        public PreviewSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescription = new SolutionDescriptionPreviewSection(solution).IfPopulated();
            Features = new FeaturesPreviewSection(solution.Features).IfPopulated();
            ClientApplicationTypes = new ClientApplicationTypesPreviewSection(solution.ClientApplication).IfPopulated();
        }
    }
}

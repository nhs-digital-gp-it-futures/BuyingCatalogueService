using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class PublicSections
    {
        [JsonProperty("solution-description")]
        public SolutionDescriptionPublicSection SolutionDescription { get; }

        public FeaturesPublicSection Features { get; }

        [JsonProperty("client-application-types")]
        public ClientApplicationTypesPublicSection ClientApplicationTypes { get; }


        [JsonProperty("contact-details")]
        public ContactDetailsPublicSection ContactDetails { get; }

        [JsonProperty("capabilities")]
        public CapabilitiesPublicSection Capabilities { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PublicSections"/> class.
        /// </summary>
        public PublicSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescription = new SolutionDescriptionPublicSection(solution).IfPopulated();
            Features = new FeaturesPublicSection(solution.Features).IfPopulated();
            ClientApplicationTypes = new ClientApplicationTypesPublicSection(solution.ClientApplication).IfPopulated();

            ContactDetails = new ContactDetailsPublicSection(solution.Contacts);
            Capabilities = new CapabilitiesPublicSection(solution.Capabilities);
        }
    }
}

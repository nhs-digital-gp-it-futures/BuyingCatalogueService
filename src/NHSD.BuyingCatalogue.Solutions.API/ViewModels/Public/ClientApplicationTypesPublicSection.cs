using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class ClientApplicationTypesPublicSection
    {
        public ClientApplicationTypesPublicSubSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypesPublicSection"/> class.
        /// </summary>
        public ClientApplicationTypesPublicSection(IClientApplication clientApplication)
        {
            Sections = new ClientApplicationTypesPublicSubSections(clientApplication);
        }

        public ClientApplicationTypesPublicSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}

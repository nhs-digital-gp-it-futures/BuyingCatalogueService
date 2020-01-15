using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications
{
    public class ClientApplicationTypesSection
    {
        public ClientApplicationTypesSubSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypesSection"/> class.
        /// </summary>
        public ClientApplicationTypesSection(IClientApplication clientApplication)
        {
            Sections = new ClientApplicationTypesSubSections(clientApplication);
        }

        public ClientApplicationTypesSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}

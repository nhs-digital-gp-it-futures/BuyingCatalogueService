using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications
{
    public sealed class ClientApplicationTypesSection
    {
        public ClientApplicationTypesSection(IClientApplication clientApplication)
        {
            Sections = new ClientApplicationTypesSubSections(clientApplication);
        }

        public ClientApplicationTypesSubSections Sections { get; }

        public ClientApplicationTypesSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class ClientApplicationTypesPreviewSection
    {
        public ClientApplicationTypesPreviewSubSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypesPreviewSection"/> class.
        /// </summary>
        public ClientApplicationTypesPreviewSection(IClientApplication clientApplication)
        {
            Sections = new ClientApplicationTypesPreviewSubSections(clientApplication);
        }

        public ClientApplicationTypesPreviewSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}

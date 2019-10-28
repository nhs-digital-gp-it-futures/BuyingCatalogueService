using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class BrowserBasedSection : Section
    {
        internal BrowserBasedSection(ClientApplication clientApplication)
        {
            Sections = new List<Section>();
            BuildSections(clientApplication);
            //_isComplete = Sections.OfType<BrowsersSupportedSection>().FirstOrDefault()?.Status == "COMPLETE"
            //    && Sections.OfType<PlugInsOrExtensionsSection>().FirstOrDefault()?.Status == "COMPLETE"
            //    && Sections.OfType<ConnectivityAndResolutionSection>().FirstOrDefault()?.Status == "COMPLETE";
        }

        private void BuildSections(ClientApplication clientApplication)
        {
            Sections.Add(new BrowsersSupportedSection(clientApplication));
            Sections.Add(new PlugInsOrExtensionsSection());
            Sections.Add(new ConnectivityAndResolutionSection());
            Sections.Add(new HardwareRequirementsSection());
            Sections.Add(new AdditionalInformationSection());
        }

        public override string Id => "browser-based";
    }
}

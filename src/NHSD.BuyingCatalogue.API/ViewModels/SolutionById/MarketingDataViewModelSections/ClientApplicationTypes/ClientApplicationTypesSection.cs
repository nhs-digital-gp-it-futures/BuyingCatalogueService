using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class ClientApplicationTypesSection : Section
    {
        internal ClientApplicationTypesSection(Solution solution)
        {
            Sections = new List<Section>();
            BuildSections(solution.ClientApplication);
            _isComplete = Sections.Any();
        }

        private void BuildSections(ClientApplication clientApplication)
        {
            if (clientApplication?.ClientApplicationTypes == null) return;

            if (clientApplication.ClientApplicationTypes.Contains("browser-based"))
            {
                Sections.Add(new BrowserBasedSection(clientApplication));
            }

            if (clientApplication.ClientApplicationTypes.Contains("native-mobile"))
            {
                Sections.Add(new NativeMobileSection());
            }

            if (clientApplication.ClientApplicationTypes.Contains("native-desktop"))
            {
                Sections.Add(new NativeDesktopSection());
            }
        }

        public override string Id => "client-application-types";
    }
}

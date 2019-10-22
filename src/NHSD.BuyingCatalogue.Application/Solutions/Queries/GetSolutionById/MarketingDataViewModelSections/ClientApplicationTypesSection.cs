using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
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
            if (clientApplication == null) return;

            if (clientApplication.ClientApplicationTypes.Contains("browser-based"))
            {
                Sections.Add(new BrowserBasedSection());
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
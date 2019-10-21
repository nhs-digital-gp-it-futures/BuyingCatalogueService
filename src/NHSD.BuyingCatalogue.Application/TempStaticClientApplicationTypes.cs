using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application
{
    public static class TempStaticClientApplicationTypes
    {
        public static ClientApplicationTypes ClientApplicationTypes { get; private set; }

        public static void SetClientApplicationTypes(IEnumerable<string> sections)
        {
            if (ClientApplicationTypes == null)
            {
                ClientApplicationTypes = new ClientApplicationTypes();
            }

            ClientApplicationTypes.BrowserBased = sections.Contains("browser-based") ? new BrowserBased() : null;
            ClientApplicationTypes.NativeMobile = sections.Contains("native-mobile") ? new NativeMobile() : null;
            ClientApplicationTypes.NativeDesktop = sections.Contains("native-desktop") ? new NativeDesktop() : null;
        }
    }
}

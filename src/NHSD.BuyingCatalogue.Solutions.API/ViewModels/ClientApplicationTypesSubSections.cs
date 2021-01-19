using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class ClientApplicationTypesSubSections
    {
        public ClientApplicationTypesSubSections(IClientApplication clientApplication)
        {
            HashSet<string> clientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();

            SetIfSelected(
                "browser-based",
                clientApplicationTypes,
                () => BrowserBasedSection = DashboardSection.Mandatory(clientApplication.IsBrowserBasedComplete()));

            SetIfSelected(
                "native-mobile",
                clientApplicationTypes,
                () => NativeMobileSection = DashboardSection.Mandatory(clientApplication.IsNativeMobileComplete()));

            SetIfSelected(
                "native-desktop",
                clientApplicationTypes,
                () => NativeDesktopSection = DashboardSection.Mandatory(clientApplication.IsNativeDesktopComplete()));
        }

        [JsonProperty("browser-based")]
        public DashboardSection BrowserBasedSection { get; private set; }

        [JsonProperty("native-mobile")]
        public DashboardSection NativeMobileSection { get; private set; }

        [JsonProperty("native-desktop")]
        public DashboardSection NativeDesktopSection { get; private set; }

        private static void SetIfSelected(string sectionName, IReadOnlySet<string> sections, Action setDashboardAction)
        {
            if (sections.Contains(sectionName))
            {
                setDashboardAction();
            }
        }
    }
}

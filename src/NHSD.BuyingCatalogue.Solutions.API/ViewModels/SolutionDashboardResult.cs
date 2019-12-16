using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class SolutionDashboardResult
    {
        public string Id { get; }

        public string Name { get; }

        [JsonProperty("sections")]
        public SolutionDashboardSections SolutionDashboardSections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDashboardResult"/> class.
        /// </summary>
        public SolutionDashboardResult(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Id = solution.Id;
            Name = solution.Name;
            SolutionDashboardSections = new SolutionDashboardSections(solution);
        }
    }

    public class SolutionDashboardSections
    {
        [JsonProperty("solution-description")]
        public DashboardSection SolutionDescriptionSection { get; }

        [JsonProperty("features")]
        public DashboardSection FeaturesSection { get; }

        [JsonProperty("client-application-types")]
        public DashboardSection ClientApplicationTypesSection { get; }

        [JsonProperty("contact-details")]
        public DashboardSection ContactDetailsSection { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDashboardSections"/> class.
        /// </summary>
        public SolutionDashboardSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescriptionSection = DashboardSection.Mandatory(!string.IsNullOrWhiteSpace(solution.Summary));
            FeaturesSection = DashboardSection.Optional(solution.Features?.Any(feature => !string.IsNullOrWhiteSpace(feature)) == true);
            ClientApplicationTypesSection = DashboardSection.MandatoryWithSubSection(
                solution.ClientApplication?.ClientApplicationTypes?.Any() == true,
                new ClientApplicationTypesSubSections(solution.ClientApplication));
            ContactDetailsSection = DashboardSection.Optional(new ContactAnswerSection(solution.Contacts).HasData());
        }
    }

    public class ClientApplicationTypesSubSections
    {
        [JsonProperty("browser-based")]
        public DashboardSection BrowserBasedSection { get; private set; }

        [JsonProperty("native-mobile")]
        public DashboardSection NativeMobileSection { get; private set; }

        [JsonProperty("native-desktop")]
        public DashboardSection NativeDesktopSection { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypesSubSections"/> class.
        /// </summary>
        /// <param name="clientApplication"></param>
        public ClientApplicationTypesSubSections(IClientApplication clientApplication)
        {
            HashSet<string> clientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();

            SetIfSelected("browser-based", clientApplicationTypes,
                () => BrowserBasedSection = DashboardSection.Mandatory(clientApplication.IsBrowserBasedComplete()));
            SetIfSelected("native-mobile", clientApplicationTypes, () => NativeMobileSection = DashboardSection.Mandatory(false));
            SetIfSelected("native-desktop", clientApplicationTypes, () => NativeDesktopSection = DashboardSection.Mandatory(false));
        }

        private void SetIfSelected(string sectionName, HashSet<string> sections, Action setDashboardAction)
        {
            if (sections.Contains(sectionName))
            {
                setDashboardAction();
            }
        }
    }

    public class DashboardSection
    {
        private readonly bool _isRequired;
        private readonly bool _isComplete;

        public string Requirement => _isRequired ? "Mandatory" : "Optional";

        public string Status => _isComplete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("sections")]
        public object Section { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DashboardSection"/> class.
        /// </summary>
        public DashboardSection(bool isRequired, bool isComplete, object section = null)
        {
            _isRequired = isRequired;
            _isComplete = isComplete;
            Section = section;
        }

        public static DashboardSection Mandatory(bool isComplete) => new DashboardSection(true, isComplete);

        public static DashboardSection Optional(bool isComplete) => new DashboardSection(false, isComplete);

        public static DashboardSection MandatoryWithSubSection(bool isComplete, object subSection) => new DashboardSection(true, isComplete, subSection);
    }
}


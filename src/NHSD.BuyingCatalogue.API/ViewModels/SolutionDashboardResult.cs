using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class SolutionDashboardResult
    {
        public SolutionDashboardResult(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Id = solution.Id;
            Name = solution.Name;

            Sections = new List<DashboardSection>
            {
                DashboardSection.Mandatory("solution-description", !string.IsNullOrWhiteSpace(solution.Summary)),
                DashboardSection.Optional("features", solution.Features?.Any(f => !string.IsNullOrWhiteSpace(f)) == true),
                DashboardSection.MandatoryWithSections("client-application-types",
                    solution.ClientApplication?.ClientApplicationTypes?.Any() == true,
                    ClientApplicationSubSections(solution))
            };
        }

        private static List<DashboardSection> ClientApplicationSubSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            var clientApplicationTypes = solution.ClientApplication?.ClientApplicationTypes ?? new HashSet<string>();
            var subSections = new List<DashboardSection>();

            AddIfSelected("browser-based", clientApplicationTypes, subSections, solution.ClientApplication?.BrowsersSupported?.Any(f => !string.IsNullOrWhiteSpace(f)) == true);
            AddIfSelected("native-mobile", clientApplicationTypes, subSections, false);
            AddIfSelected("native-desktop", clientApplicationTypes, subSections, false);

            return subSections;
        }

        private static void AddIfSelected(string sectionName, HashSet<string> clientApplicationTypes, List<DashboardSection> subSections, bool isComplete)
        {
            if (clientApplicationTypes.Contains(sectionName))
            {
                subSections.Add(DashboardSection.Mandatory(sectionName, isComplete));
            }
        }

        public string Id { get; }

        public string Name { get; }

        public IEnumerable<DashboardSection> Sections { get; }
    }

    public class DashboardSection
    {
        private readonly bool _isRequired;
        private readonly bool _isComplete;

        public string Id { get; }

        public string Requirement => _isRequired ? "Mandatory" : "Optional";

        public string Status => _isComplete ? "COMPLETE" : "INCOMPLETE";

        public IEnumerable<DashboardSection> Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DashboardSection"/> class.
        /// </summary>
        public DashboardSection(string id, bool isRequired, bool isComplete, IEnumerable<DashboardSection> dashboardSections = null)
        {
            Id = id;
            _isRequired = isRequired;
            _isComplete = isComplete;
            Sections = dashboardSections;
        }

        public static DashboardSection MandatoryWithSections(string id, bool isComplete, IEnumerable<DashboardSection> dashboardSections) => new DashboardSection(id, true, isComplete, dashboardSections);

        public static DashboardSection Mandatory(string id, bool isComplete) => new DashboardSection(id, true, isComplete);

        public static DashboardSection Optional(string id, bool isComplete) => new DashboardSection(id, false, isComplete);
    }
}


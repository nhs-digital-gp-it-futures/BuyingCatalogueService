using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// A view representation of the <see cref="Solution"/> entity that matched a specific ID.
    /// </summary>
    public sealed class SolutionByIdViewModel
    {
        internal SolutionByIdViewModel(Solution solution)
        {
            Id = solution.Id;
            Name = solution.Name;
            MarketingData = new MarketingDataViewModel(solution);
        }

        /// <summary>
        /// Identifier of the solution.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the solution.
        /// </summary>
        public string Name { get; }

        public MarketingDataViewModel MarketingData { get; }
    }

    public sealed class MarketingDataViewModel
    {
        internal MarketingDataViewModel(Solution solution)
        {
            Sections = new List<Section>
            {
                new SolutionDescriptionSection(solution),
                new FeaturesSection(solution),
                new ClientApplicationTypesSection(solution)
            };
        }

        public IEnumerable<Section> Sections { get; }
    }

    public abstract class Section
    {
        protected bool _isComplete = false;

        public abstract string Id { get; }

        public string Requirement => (Mandatory.Any() || Sections != null) ? "Mandatory" : "Optional";

        public string Status => _isComplete ? "COMPLETE" : "INCOMPLETE";

        public List<string> Mandatory = new List<string>();

        public List<Section> Sections { get; protected set; } //Subsections - may be null;

    }

    public class SolutionDescriptionSection : Section
    {
        internal SolutionDescriptionSection(Solution solution)
        {
            Data = new SolutionDescription(solution);
            _isComplete = !string.IsNullOrWhiteSpace(solution.Summary);
            Mandatory.Add("summary");
        }

        public override string Id => "solution-description";

        public SolutionDescription Data { get; }
    }

    public class SolutionDescription
    {
        internal SolutionDescription(Solution solution)
        {
            Summary = solution.Summary;
            Description = solution.Description;
            Link = solution.AboutUrl;
        }

        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }
    }

    public class FeaturesSection : Section
    {
        internal FeaturesSection(Solution solution)
        {
            Data = new Features(solution.Features);
            _isComplete = Data.Listing.Any(s => !string.IsNullOrWhiteSpace(s));
        }

        public override string Id => "features";

        public Features Data { get; }
    }

    public class Features
    {
        internal Features(string featuresJson)
        {
            Listing = string.IsNullOrWhiteSpace(featuresJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(featuresJson);
        }

        public IEnumerable<string> Listing { get; }
    }

    public class ClientApplicationTypesSection : Section
    {
        internal ClientApplicationTypesSection(Solution solution)
        {
            Sections = new List<Section>();

            //temp - ignore the solution, return canned data
            BuildCannedSections(solution.ClientApplicationTypes);
            _isComplete = Sections.Any();
        }

        private void BuildCannedSections(ClientApplicationTypes clientApplicationTypes)
        {
            if(clientApplicationTypes == null) return;

            if (clientApplicationTypes.BrowserBased != null)
            {
                Sections.Add(new BrowserBasedSection(clientApplicationTypes.BrowserBased));
            }

            if (clientApplicationTypes.NativeMobile != null)
            {
                Sections.Add(new NativeMobileSection());
            }

            if (clientApplicationTypes.NativeDesktop != null)
            {
                Sections.Add(new NativeDesktopSection());
            }
        }

        public override string Id => "client-application-types";
    }

    public class BrowserBasedSection : Section
    {
        internal BrowserBasedSection(BrowserBased browserBased)
        {
            if (browserBased == null) return;
            //Sections = new List<Section>();

            ////temp - ignore the solution, return canned data
            //BuildCannedSections();
            //_isComplete = Sections.OfType<BrowsersSupportedSection>().FirstOrDefault()?.Status == "COMPLETE"
            //    && Sections.OfType<PlugInsOrExtensionsSection>().FirstOrDefault()?.Status == "COMPLETE"
            //    && Sections.OfType<ConnectivityAndResolutionSection>().FirstOrDefault()?.Status == "COMPLETE";
        }

        private void BuildCannedSections()
        {
            Sections.Add(new BrowsersSupportedSection());
            Sections.Add(new PlugInsOrExtensionsSection());
            Sections.Add(new ConnectivityAndResolutionSection());
            Sections.Add(new HardwareRequirementsSection());
            Sections.Add(new AdditionalInformationSection());
        }

        public override string Id => "browser-based";
    }

    public class BrowsersSupportedSection : Section
    {
        internal BrowsersSupportedSection()
        {
            Data = new BrowserSupportedSectionData();

            _isComplete = Data.GoogleChrome ||
                          Data.MicrosoftEdge ||
                          Data.MozillaFirefox ||
                          Data.Opera ||
                          Data.Safari ||
                          Data.Chromium ||
                          Data.InternetExplorer11 ||
                          Data.InternetExplorer10;
        }

        public BrowserSupportedSectionData Data { get; }

        public override string Id => "browsers-supported";
    }

    public class BrowserSupportedSectionData
    {
        public bool GoogleChrome => false;
        public bool MicrosoftEdge => true;
        public bool MozillaFirefox => false;
        public bool Opera => true;
        public bool Safari => true;
        public bool Chromium => false;
        public bool InternetExplorer11 => true;
        public bool InternetExplorer10 => false;
    }

    public class PlugInsOrExtensionsSection : Section
    {
        public override string Id => "plug-ins-or-extensions";
    }

    public class ConnectivityAndResolutionSection : Section
    {
        public override string Id => "connectivity-and-resolution";
    }

    public class HardwareRequirementsSection : Section
    {
        public override string Id => "hardware-requirements";
    }

    public class AdditionalInformationSection : Section
    {
    public override string Id => "additional-information";
    }

    public class NativeMobileSection : Section
    {
        public override string Id => "native-mobile";
    }

    public class NativeDesktopSection : Section
    {
        public override string Id => "native-desktop";
    }
}

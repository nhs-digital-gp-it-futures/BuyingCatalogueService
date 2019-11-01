using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class SolutionPreviewResult
    {
        public SolutionPreviewResult(Solution solution)
        {
            Id = solution.Id;
            Name = solution.Name;
            OrganisationName = solution.OrganisationName;
            Sections = new PreviewSections(solution);
        }

        public string Id { get; }

        public string Name { get; }

        public string OrganisationName { get; }

        public PreviewSections Sections { get; }
    }

    public class PreviewSections
    {
        public PreviewSections(Solution solution)
        {
            SolutionDescription = new SolutionDescriptionSection(solution).IfPopulated();
            Features = new FeaturesSection(solution.Features).IfPopulated();
            ClientApplicationTypes = new ClientApplicationTypesSection(solution.ClientApplication).IfPopulated();
        }

        [JsonProperty("solution-description")]
        public SolutionDescriptionSection SolutionDescription { get; }

        public FeaturesSection Features { get; }

        [JsonProperty("client-application-types")]
        public ClientApplicationTypesSection ClientApplicationTypes { get; }
    }

    public class SolutionDescriptionSection
    {
        public SolutionDescriptionSection(Solution solution)
        {
            Answers = new SolutionDescriptionSectionAnswers(solution);
        }

        public SolutionDescriptionSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }

        public SolutionDescriptionSectionAnswers Answers { get; }
    }

    public class SolutionDescriptionSectionAnswers
    {
        public SolutionDescriptionSectionAnswers(Solution solution)
        {
            Summary = solution.Summary;
            Description = solution.Description;
            Link = solution.AboutUrl;
        }

        [JsonIgnore]
        public bool HasData => !(string.IsNullOrWhiteSpace(Summary)
                               && string.IsNullOrWhiteSpace(Description)
                               && string.IsNullOrWhiteSpace(Link));

        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }
    }

    public class FeaturesSection
    {
        public FeaturesSection(IEnumerable<string> features)
        {
            Answers = new FeaturesSectionAnswers(features);
        }

        public FeaturesSectionAnswers Answers { get; }

        public FeaturesSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }

    public class FeaturesSectionAnswers
    {
        public FeaturesSectionAnswers(IEnumerable<string> features)
        {
            Listing = features;
        }

        public IEnumerable<string> Listing { get; }

        [JsonIgnore]
        public bool HasData => Listing?.Any() == true;
    }

    public class ClientApplicationTypesSection
    {
        public ClientApplicationTypesSection(ClientApplication clientApplication)
        {
            Sections = new ClientApplicationTypesSections(clientApplication);
        }

        public ClientApplicationTypesSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }

        public ClientApplicationTypesSections Sections { get; }
    }

    public class ClientApplicationTypesSections
    {
        public ClientApplicationTypesSections(ClientApplication clientApplication)
        {
            BrowserBased = new BrowserBasedSection(clientApplication).IfPopulated();
        }

        [JsonProperty("browser-based")]
        public BrowserBasedSection BrowserBased { get; }

        [JsonIgnore]
        public bool HasData => BrowserBased != null;
    }

    public class BrowserBasedSection
    {
        public BrowserBasedSection(ClientApplication clientApplication)
        {
            Sections = new BrowserBasedSections(clientApplication);
        }

        public BrowserBasedSection IfPopulated()
        {
            return Sections.BrowsersSupported.Answers.HasData ? this : null;
        }

        [JsonProperty("sections")]
        public BrowserBasedSections Sections { get; }
    }

    public class BrowserBasedSections
    {
        public BrowserBasedSections(ClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedSection(clientApplication);
        }

        [JsonProperty("browsers-supported")]
        public BrowsersSupportedSection BrowsersSupported { get; }
    }

    public class BrowsersSupportedSection
    {
        public BrowsersSupportedSection(ClientApplication clientApplication)
        {
            Answers = new BrowsersSupportedSectionAnswers(clientApplication);
        }

        public BrowsersSupportedSectionAnswers Answers { get; }
    }

    public class BrowsersSupportedSectionAnswers
    {
        public BrowsersSupportedSectionAnswers(ClientApplication clientApplication)
        {
            SupportedBrowsers = clientApplication?.BrowsersSupported;
            MobileResponsive = clientApplication?.MobileResponsive.HasValue == true
                ? (clientApplication.MobileResponsive.Value ? "yes" : "no")
                : null;
        }

        [JsonProperty("supported-browsers")]
        public IEnumerable<string> SupportedBrowsers { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

        [JsonIgnore]
        public bool HasData => SupportedBrowsers?.Any() == true || MobileResponsive != null;
    }
}

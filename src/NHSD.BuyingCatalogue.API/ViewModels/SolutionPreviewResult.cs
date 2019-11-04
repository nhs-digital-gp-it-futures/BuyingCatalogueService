using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class SolutionPreviewResult
    {
        public string Id { get; }

        public string Name { get; }

        public string OrganisationName { get; }

        public PreviewSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionPreviewResult"/> class.
        /// </summary>
        public SolutionPreviewResult(Solution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Id = solution.Id;
            Name = solution.Name;
            OrganisationName = solution.OrganisationName;
            Sections = new PreviewSections(solution);
        }
    }

    public class PreviewSections
    {
        [JsonProperty("solution-description")]
        public SolutionDescriptionSection SolutionDescription { get; }

        public FeaturesSection Features { get; }

        [JsonProperty("client-application-types")]
        public ClientApplicationTypesSection ClientApplicationTypes { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PreviewSections"/> class.
        /// </summary>
        public PreviewSections(Solution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescription = new SolutionDescriptionSection(solution).IfPopulated();
            Features = new FeaturesSection(solution.Features).IfPopulated();
            ClientApplicationTypes = new ClientApplicationTypesSection(solution.ClientApplication).IfPopulated();
        }
    }

    public class SolutionDescriptionSection
    {
        public SolutionDescriptionSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionSection"/> class.
        /// </summary>
        public SolutionDescriptionSection(Solution solution)
        {
            Answers = new SolutionDescriptionSectionAnswers(solution);
        }

        public SolutionDescriptionSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }

    public class SolutionDescriptionSectionAnswers
    {
        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }

        [JsonIgnore]
        public bool HasData => !(string.IsNullOrWhiteSpace(Summary)
                                 && string.IsNullOrWhiteSpace(Description)
                                 && string.IsNullOrWhiteSpace(Link));

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionSectionAnswers"/> class.
        /// </summary>
        public SolutionDescriptionSectionAnswers(Solution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Summary = solution.Summary;
            Description = solution.Description;
            Link = solution.AboutUrl;
        }
    }

    public class FeaturesSection
    {
        public FeaturesSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesSection"/> class.
        /// </summary>
        public FeaturesSection(IEnumerable<string> features)
        {
            Answers = new FeaturesSectionAnswers(features);
        }

        public FeaturesSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }

    public class FeaturesSectionAnswers
    {
        public IEnumerable<string> Listing { get; }

        [JsonIgnore]
        public bool HasData => Listing?.Any() == true;

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesSectionAnswers"/> class.
        /// </summary>
        public FeaturesSectionAnswers(IEnumerable<string> features)
        {
            Listing = features;
        }
    }

    public class ClientApplicationTypesSection
    {
        public ClientApplicationTypesSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypesSection"/> class.
        /// </summary>
        public ClientApplicationTypesSection(ClientApplication clientApplication)
        {
            Sections = new ClientApplicationTypesSections(clientApplication);
        }

        public ClientApplicationTypesSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
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
        [JsonProperty("sections")]
        public BrowserBasedSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedSection"/> class.
        /// </summary>
        public BrowserBasedSection(ClientApplication clientApplication)
        {
            Sections = new BrowserBasedSections(clientApplication);
        }

        public BrowserBasedSection IfPopulated()
        {
            return Sections.BrowsersSupported.Answers.HasData ? this : null;
        }
    }

    public class BrowserBasedSections
    {
        [JsonProperty("browsers-supported")]
        public BrowsersSupportedSection BrowsersSupported { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedSections"/> class.
        /// </summary>
        public BrowserBasedSections(ClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedSection(clientApplication);
        }
    }

    public class BrowsersSupportedSection
    {
        public BrowsersSupportedSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedSection"/> class.
        /// </summary>
        public BrowsersSupportedSection(ClientApplication clientApplication)
        {
            Answers = new BrowsersSupportedSectionAnswers(clientApplication);
        }
    }

    public class BrowsersSupportedSectionAnswers
    {
        [JsonProperty("supported-browsers")]
        public IEnumerable<string> SupportedBrowsers { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

        [JsonIgnore]
        public bool HasData => SupportedBrowsers?.Any() == true || MobileResponsive != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedSectionAnswers"/> class.
        /// </summary>
        public BrowsersSupportedSectionAnswers(ClientApplication clientApplication)
        {
            bool? mobileResponsive = clientApplication?.MobileResponsive;

            SupportedBrowsers = clientApplication?.BrowsersSupported;
            MobileResponsive = mobileResponsive.HasValue
                ? mobileResponsive.Value ? "yes" : "no"
                : null;
        }
    }
}

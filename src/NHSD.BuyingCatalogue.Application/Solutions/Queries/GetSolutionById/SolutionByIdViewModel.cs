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
                new FeaturesSection(solution)
            };
        }

        public IEnumerable<Section> Sections { get; }
    }

    public abstract class Section
    {
        protected bool _isComplete = false;
        protected bool _isMandatory = false;

        public abstract string Id { get; }

        public string Requirement => _isMandatory ? "Mandatory" : "Optional";

        public string Status => _isComplete ? "COMPLETE" : "INCOMPLETE";
    }

    public class SolutionDescriptionSection : Section
    {
        internal SolutionDescriptionSection(Solution solution)
        {
            Data = new SolutionDescription(solution);
            _isComplete = !string.IsNullOrWhiteSpace(solution.Summary);
            _isMandatory = true;
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
            _isMandatory = false;
        }

        public override string Id => "features";

        public Features Data { get; }
    }

    public class Features
    {
        internal Features(string featuresJson)
        {
            Listing = string.IsNullOrWhiteSpace(featuresJson) ?
                new List<string>() : 
                JsonConvert.DeserializeObject<List<string>>(featuresJson);
        }

        public IEnumerable<string> Listing { get; }
    }
}

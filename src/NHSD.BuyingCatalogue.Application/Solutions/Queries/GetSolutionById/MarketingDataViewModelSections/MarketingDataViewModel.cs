using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
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
}
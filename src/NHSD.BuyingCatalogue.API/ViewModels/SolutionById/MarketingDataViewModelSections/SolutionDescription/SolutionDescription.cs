using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
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
}

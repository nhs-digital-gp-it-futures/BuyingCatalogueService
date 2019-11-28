using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SolutionResult : ISolutionResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LastUpdated { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string AboutUrl { get; set; }

        public string Features { get; set; }

        public string ClientApplication { get; set; }

        public string OrganisationName { get; set; }

        public bool IsFoundation { get; set; }
    }
}
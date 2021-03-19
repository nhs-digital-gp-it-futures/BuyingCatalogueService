using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class SolutionFramework
    {
        internal SolutionFramework(ISolutionFrameworkListResult solutionFrameworkListResult)
        {
            Id = solutionFrameworkListResult.Id;
            ShortName = solutionFrameworkListResult.ShortName;
        }

        public string Id { get; }

        public string ShortName { get; set; }
    }
}

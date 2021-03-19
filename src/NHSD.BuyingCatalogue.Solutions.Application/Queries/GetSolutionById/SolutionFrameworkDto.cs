using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal sealed class SolutionFrameworkDto : ISolutionFramework
    {
        public string Id { get; set; }

        public string ShortName { get; set; }
    }
}

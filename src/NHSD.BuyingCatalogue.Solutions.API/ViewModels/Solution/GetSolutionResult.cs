using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class GetSolutionResult
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public GetSolutionResult(ISolution result)
        {
            Name = result?.Name;
            Summary = result?.Summary;
        }
    }
}

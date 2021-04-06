using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class GetSolutionResult
    {
        public GetSolutionResult(ISolution result)
        {
            Name = result?.Name;
            Summary = result?.Summary;
            IsFoundation = result?.IsFoundation;
        }

        public string Name { get; set; }

        public string Summary { get; set; }

        public bool? IsFoundation { get; set; }
    }
}

using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class SolutionSupplier
    {
        public SolutionSupplier()
        {
        }

        internal SolutionSupplier(ISolutionSupplierResult solutionSupplierResult)
        {
            Name = solutionSupplierResult.Name;
            Summary = solutionSupplierResult.Summary;
            Url = solutionSupplierResult.Url;
        }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Url { get; set; }
    }
}

using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionSupplierResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionSupplierResult"/> class.
        /// </summary>
        /// <param name="supplier">The solution supplier.</param>
        public SolutionSupplierResult(ISolutionSupplier supplier)
        {
            Id = supplier?.Id;
            Name = supplier?.Name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}

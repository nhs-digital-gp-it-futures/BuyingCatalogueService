using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionSupplierResult
    {
        public string Id { get; }

        public string Name { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionSupplierResult"/> class.
        /// </summary>
        public SolutionSupplierResult(ISolutionSupplier supplier)
        {
            Id = supplier?.Id;
            Name = supplier?.Name;
        }
    }
}

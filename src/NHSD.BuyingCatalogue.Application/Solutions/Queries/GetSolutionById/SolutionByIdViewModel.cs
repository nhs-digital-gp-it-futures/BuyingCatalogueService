
namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// A view representation of the <see cref="Solution"/> entity that matched a specific ID.
    /// </summary>
    public sealed class SolutionByIdViewModel
    {
        /// <summary>
        /// Identifier of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Marketing data of the solution.
        /// </summary>
        public string MarketingData { get; set; }
    }
}

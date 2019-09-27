
using Newtonsoft.Json.Linq;

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
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Marketing data of the solution.
        /// </summary>
        public JObject MarketingData { get; set; }

        /// <summary>
        /// A link to provide more information about a solution.
        /// </summary>
        public string AboutUrl { get; set; }
    }
}

using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionSummaryViewModel : IUpdateSolutionSummary
    {
        /// <summary>
        /// Description of the solution.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A link to more information regarding the solution.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        public string Summary { get; set; }
    }
}

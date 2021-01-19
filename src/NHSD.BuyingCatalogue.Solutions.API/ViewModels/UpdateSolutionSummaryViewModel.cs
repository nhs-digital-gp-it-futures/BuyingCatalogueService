using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionSummaryViewModel : IUpdateSolutionSummary
    {
        /// <summary>
        /// Gets or sets the description of the solution.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the a link to more information regarding the solution.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the summary of the solution.
        /// </summary>
        public string Summary { get; set; }
    }
}

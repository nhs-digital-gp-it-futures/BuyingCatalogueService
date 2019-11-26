namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    public sealed class UpdateSolutionSummaryViewModel
    {
        /// <summary>
        /// Description of the solution.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A link to more information regarding the solution.
        /// </summary>
        public string Link { get; set; }
    }
}

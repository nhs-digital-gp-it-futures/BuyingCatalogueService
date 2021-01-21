namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateSolutionSummary
    {
        /// <summary>
        /// Gets the description of the solution.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a link to more information regarding the solution.
        /// </summary>
        string Link { get; }

        /// <summary>
        /// Gets the summary of the solution.
        /// </summary>
        string Summary { get; }
    }
}

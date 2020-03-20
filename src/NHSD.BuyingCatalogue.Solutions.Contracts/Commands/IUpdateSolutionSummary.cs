namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateSolutionSummary
    {
        /// <summary>
        /// Description of the solution.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// A link to more information regarding the solution.
        /// </summary>
        string Link { get; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        string Summary { get; }
    }
}

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    /// <summary>
    /// Represents the result for the <see cref="GetSolutionByIdQuery"/>.
    /// </summary>
    public sealed class GetSolutionByIdResult
    {
        /// <summary>
        /// The details of the matching solution. If the solution could not be found, then return null.
        /// </summary>
        public SolutionByIdViewModel Solution { get; }

        public GetSolutionByIdResult(SolutionByIdViewModel solution) => Solution = solution;
    }
}

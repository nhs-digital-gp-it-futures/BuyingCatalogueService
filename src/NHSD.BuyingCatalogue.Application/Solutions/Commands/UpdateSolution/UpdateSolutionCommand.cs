using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionCommand : IRequest
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; set; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public UpdateSolutionViewModel UpdateSolutionViewModel { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionCommand"/> class.
        /// </summary>
        public UpdateSolutionCommand(string solutionId, UpdateSolutionViewModel updateSolutionViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionViewModel = updateSolutionViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionViewModel));
        }
    }
}

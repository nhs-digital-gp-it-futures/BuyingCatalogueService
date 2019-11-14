using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary
{
    public sealed class UpdateSolutionSummaryCommand : IRequest<UpdateSolutionSummaryValidationResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; set; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public UpdateSolutionSummaryViewModel UpdateSolutionSummaryViewModel { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionSummaryCommand"/> class.
        /// </summary>
        public UpdateSolutionSummaryCommand(string solutionId, UpdateSolutionSummaryViewModel updateSolutionSummaryViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionSummaryViewModel = updateSolutionSummaryViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionSummaryViewModel));
        }
    }
}

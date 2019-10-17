using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionFeaturesCommand : IRequest
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; set; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public UpdateSolutionFeaturesViewModel UpdateSolutionFeaturesViewModel { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionFeaturesCommand"/> class.
        /// </summary>
        public UpdateSolutionFeaturesCommand(string solutionId, UpdateSolutionFeaturesViewModel updateSolutionFeaturesViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionFeaturesViewModel = updateSolutionFeaturesViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionFeaturesViewModel));
        }
    }
}

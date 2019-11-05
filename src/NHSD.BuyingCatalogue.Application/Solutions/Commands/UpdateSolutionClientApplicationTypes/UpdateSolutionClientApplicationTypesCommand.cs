using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes
{
    public sealed class UpdateSolutionClientApplicationTypesCommand : IRequest<UpdateSolutionClientApplicationTypesValidationResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; set; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public UpdateSolutionClientApplicationTypesViewModel UpdateSolutionClientApplicationTypesViewModel { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionClientApplicationTypesCommand"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesCommand(string solutionId, UpdateSolutionClientApplicationTypesViewModel updateSolutionClientApplicationTypesViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionClientApplicationTypesViewModel = updateSolutionClientApplicationTypesViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionClientApplicationTypesViewModel));
        }
    }
}

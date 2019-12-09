using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    public class UpdateSolutionContactDetailsCommand : IRequest<UpdateSolutionContactDetailsValidationResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated contact details for a solution
        /// </summary>
        public UpdateSolutionContactDetailsViewModel Details { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionContactDetailsCommand"/> class.
        /// </summary>
        public UpdateSolutionContactDetailsCommand(string solutionId, UpdateSolutionContactDetailsViewModel commandDetails)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            Details = commandDetails ?? throw new System.ArgumentNullException(nameof(commandDetails));
        }
    }
}

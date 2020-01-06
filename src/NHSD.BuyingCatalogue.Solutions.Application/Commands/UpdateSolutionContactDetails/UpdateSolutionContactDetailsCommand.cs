using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    public class UpdateSolutionContactDetailsCommand : IRequest<ContactsMaxLengthResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated contact details for a solution
        /// </summary>
        public UpdateSolutionContactDetailsViewModel Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionContactDetailsCommand"/> class.
        /// </summary>
        public UpdateSolutionContactDetailsCommand(string solutionId, UpdateSolutionContactDetailsViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.Contact1?.Trim();
            Data.Contact2?.Trim();
        }
    }
}

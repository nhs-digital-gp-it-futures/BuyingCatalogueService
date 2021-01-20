using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    public class UpdateSolutionContactDetailsCommand : IRequest<ContactsMaxLengthResult>
    {
        /// <summary>
        /// Gets a value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Gets the updated contact details for a solution.
        /// </summary>
        public IUpdateSolutionContactDetails Data { get; }

        public UpdateSolutionContactDetailsCommand(string solutionId, IUpdateSolutionContactDetails data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

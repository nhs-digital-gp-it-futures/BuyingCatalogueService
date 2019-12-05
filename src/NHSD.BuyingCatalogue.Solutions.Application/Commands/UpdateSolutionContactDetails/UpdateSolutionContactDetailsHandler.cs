using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    class UpdateSolutionContactDetailsHandler : IRequestHandler<UpdateSolutionContactDetailsCommand, UpdateSolutionContactDetailsValidationResult>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SolutionContactDetailsUpdater _updater;
        private readonly UpdateSolutionContactDetailsValidator _validator;

        public UpdateSolutionContactDetailsHandler(SolutionVerifier verifier, SolutionContactDetailsUpdater updater, UpdateSolutionContactDetailsValidator validator)
        {
            _verifier = verifier;
            _updater = updater;
            _validator = validator;
        }

        public async Task<UpdateSolutionContactDetailsValidationResult> Handle(UpdateSolutionContactDetailsCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissing(request.SolutionId, cancellationToken);

            var validationResult = _validator.Validation(request.Details);

            if (validationResult.IsValid)
            {
                await _updater.UpdateAsync(request.SolutionId, MapContacts(request.Details), cancellationToken);
            }

            return validationResult;
        }

        private static IEnumerable<IContact> MapContacts(UpdateSolutionContactDetailsViewModel details)
            => new List<IContact> { ToContact(details.Contact1), ToContact(details.Contact2) }.Where(x => x != null);

        private static IContact ToContact(UpdateSolutionContactViewModel contact)
        {
            return contact?.HasData() == true ? new ContactDto
                {
                    Department = contact.Department,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber
                }
                : null;
        }
    }
}

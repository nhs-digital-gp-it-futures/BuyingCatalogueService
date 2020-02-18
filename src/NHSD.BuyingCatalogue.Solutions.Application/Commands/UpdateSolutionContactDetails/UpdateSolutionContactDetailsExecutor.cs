using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal class UpdateSolutionContactDetailsExecutor : IExecutor<UpdateSolutionContactDetailsCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SolutionContactDetailsUpdater _updater;

        public UpdateSolutionContactDetailsExecutor(SolutionVerifier verifier, SolutionContactDetailsUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateSolutionContactDetailsCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);

            await _updater.UpdateAsync(request.SolutionId, MapContacts(request.Data), cancellationToken).ConfigureAwait(false);
        }

        private static IEnumerable<IContact> MapContacts(IUpdateSolutionContactDetails details)
            => new List<IContact> { ToContact(details.Contact1), ToContact(details.Contact2) }.Where(x => x != null);

        private static IContact ToContact(IUpdateSolutionContact contact) =>
            contact?.HasData() == true ? new ContactDto
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

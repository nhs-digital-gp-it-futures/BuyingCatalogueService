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
    internal sealed class UpdateSolutionContactDetailsExecutor : IExecutor<UpdateSolutionContactDetailsCommand>
    {
        private readonly SolutionVerifier verifier;
        private readonly SolutionContactDetailsUpdater updater;

        public UpdateSolutionContactDetailsExecutor(SolutionVerifier verifier, SolutionContactDetailsUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateSolutionContactDetailsCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);

            await updater.UpdateAsync(request.SolutionId, MapContacts(request.Data), cancellationToken);
        }

        private static IEnumerable<IContact> MapContacts(IUpdateSolutionContactDetails details) =>
            new List<IContact> { ToContact(details.Contact1), ToContact(details.Contact2) }.Where(c => c is not null);

        private static IContact ToContact(IUpdateSolutionContact contact) =>
            contact?.HasData() == true
                ? new ContactDto
                {
                    Department = contact.Department,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber,
                }
                : null;
    }
}

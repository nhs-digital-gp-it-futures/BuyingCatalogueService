using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    internal class UpdateSolutionMobileOperatingSystemsExecutor : IExecutor<UpdateSolutionMobileOperatingSystemsCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionMobileOperatingSystemsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionMobileOperatingSystemsCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.Id,
                clientApplication =>
                {
                    clientApplication.MobileOperatingSystems = new MobileOperatingSystems()
                    {
                        OperatingSystems = request.Data.OperatingSystems,
                        OperatingSystemsDescription = request.Data.OperatingSystemsDescription
                    };
                }, cancellationToken).ConfigureAwait(false);
    }
}

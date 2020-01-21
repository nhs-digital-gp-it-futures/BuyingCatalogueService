using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise
{
    internal sealed class UpdateOnPremiseExecutor : IExecutor<UpdateOnPremiseCommand>
    {
        private readonly HostingPartialUpdater _hostingUpdater;

        public UpdateOnPremiseExecutor(HostingPartialUpdater hostingUpdater) =>
            _hostingUpdater = hostingUpdater;

        public async Task UpdateAsync(UpdateOnPremiseCommand request, CancellationToken cancellationToken) =>
            await _hostingUpdater.UpdateAsync(request.Id, hosting =>
                {
                    hosting.OnPremise = new Domain.OnPremise
                    {
                        Summary = request.Data.Summary,
                        Link = request.Data.Link,
                        HostingModel = request.Data.HostingModel,
                        RequiresHSCN = request.Data.RequiresHSCN.FirstOrDefault()
                    };
                },
                cancellationToken).ConfigureAwait(false);
    }
}

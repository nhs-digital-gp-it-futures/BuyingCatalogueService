using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud
{
    internal sealed class UpdatePrivateCloudExecutor : IExecutor<UpdatePrivateCloudCommand>
    {
        private readonly HostingPartialUpdater _hostingUpdater;

        public UpdatePrivateCloudExecutor(HostingPartialUpdater hostingUpdater) =>
            _hostingUpdater = hostingUpdater;

        public async Task UpdateAsync(UpdatePrivateCloudCommand request, CancellationToken cancellationToken) =>
            await _hostingUpdater.UpdateAsync(request.Id, hosting =>
                {
                    hosting.PrivateCloud = new Domain.PrivateCloud
                    {
                        Summary = request.Data.Summary,
                        Link = request.Data.Link,
                        HostingModel = request.Data.HostingModel,
                        RequiresHSCN = request.Data.RequiresHSCN
                    };
                },
                cancellationToken).ConfigureAwait(false);
    }
}

using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.PrivateCloud
{
    internal sealed class UpdatePrivateCloudExecutor : IExecutor<UpdatePrivateCloudCommand>
    {
        private readonly HostingPartialUpdater hostingUpdater;

        public UpdatePrivateCloudExecutor(HostingPartialUpdater hostingUpdater) =>
            this.hostingUpdater = hostingUpdater;

        public async Task UpdateAsync(UpdatePrivateCloudCommand request, CancellationToken cancellationToken) =>
            await hostingUpdater.UpdateAsync(
                request.Id,
                hosting =>
                {
                    hosting.PrivateCloud = new Domain.PrivateCloud
                    {
                        Summary = request.Data.Summary,
                        Link = request.Data.Link,
                        HostingModel = request.Data.HostingModel,
                        RequiresHscn = request.Data.RequiresHscn,
                    };
                },
                cancellationToken);
    }
}

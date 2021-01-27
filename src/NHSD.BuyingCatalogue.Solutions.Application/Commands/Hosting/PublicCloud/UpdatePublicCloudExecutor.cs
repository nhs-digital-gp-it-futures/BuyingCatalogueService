using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.PublicCloud
{
    internal sealed class UpdatePublicCloudExecutor : IExecutor<UpdatePublicCloudCommand>
    {
        private readonly HostingPartialUpdater hostingPartialUpdater;

        public UpdatePublicCloudExecutor(HostingPartialUpdater hostingPartialUpdater) =>
            this.hostingPartialUpdater = hostingPartialUpdater;

        public async Task UpdateAsync(UpdatePublicCloudCommand request, CancellationToken cancellationToken) =>
            await hostingPartialUpdater.UpdateAsync(
                request.SolutionId,
                hosting =>
                {
                    hosting.PublicCloud = new Domain.HostingTypes.PublicCloud
                    {
                        Summary = request.Data.Summary,
                        Link = request.Data.Link,
                        RequiresHSCN = request.Data.RequiresHSCN,
                    };
                },
                cancellationToken);
    }
}

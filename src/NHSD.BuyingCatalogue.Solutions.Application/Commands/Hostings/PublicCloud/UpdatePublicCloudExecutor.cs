using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud
{
    internal sealed class UpdatePublicCloudExecutor : IExecutor<UpdatePublicCloudCommand>
    {
        private readonly HostingPartialUpdater _hostingPartialUpdater;

        public UpdatePublicCloudExecutor(HostingPartialUpdater hostingPartialUpdater) =>
            _hostingPartialUpdater = hostingPartialUpdater;

        public async Task UpdateAsync(UpdatePublicCloudCommand request, CancellationToken cancellationToken) =>
            await _hostingPartialUpdater.UpdateAsync(request.SolutionId, hosting =>
                {
                    hosting.PublicCloud = new Domain.Hostings.PublicCloud
                    {
                        Summary = request.Data.Summary,
                        Link = request.Data.Link,
                        RequiresHSCN = request.Data.RequiresHSCN
                    };
                },
                cancellationToken).ConfigureAwait(false);
    }
}

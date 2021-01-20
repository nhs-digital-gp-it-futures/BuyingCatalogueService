using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.HybridHostingType
{
    internal sealed class UpdateHybridHostingTypeExecutor : IExecutor<UpdateHybridHostingTypeCommand>
    {
        private readonly HostingPartialUpdater hostingUpdater;

        public UpdateHybridHostingTypeExecutor(HostingPartialUpdater hostingUpdater) =>
            this.hostingUpdater = hostingUpdater;

        public async Task UpdateAsync(UpdateHybridHostingTypeCommand request, CancellationToken cancellationToken) =>
            await hostingUpdater.UpdateAsync(
                request.Id,
                hosting =>
                {
                    hosting.HybridHostingType = new Domain.Hostings.HybridHostingType
                    {
                        Summary = request.Data.Summary,
                        Link = request.Data.Link,
                        HostingModel = request.Data.HostingModel,
                        RequiresHSCN = request.Data.RequiresHSCN,
                    };
                },
                cancellationToken);
    }
}

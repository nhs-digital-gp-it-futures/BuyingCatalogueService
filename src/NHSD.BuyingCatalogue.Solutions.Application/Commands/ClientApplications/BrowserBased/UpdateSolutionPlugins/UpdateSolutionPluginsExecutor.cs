using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsExecutor : IExecutor<UpdateSolutionPluginsCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionPluginsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionPluginsCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.Plugins = new Plugins
                    {
                        Required = request.Data.Required.ToBoolean(),
                        AdditionalInformation = request.Data.AdditionalInformation,
                    };
                },
                cancellationToken);
    }
}

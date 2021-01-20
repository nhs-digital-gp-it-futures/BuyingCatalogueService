﻿using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation
{
    internal sealed class UpdateBrowserBasedAdditionalInformationExecutor : IExecutor<UpdateBrowserBasedAdditionalInformationCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateBrowserBasedAdditionalInformationExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateBrowserBasedAdditionalInformationCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.AdditionalInformation = request.AdditionalInformation;
                },
                cancellationToken);
    }
}

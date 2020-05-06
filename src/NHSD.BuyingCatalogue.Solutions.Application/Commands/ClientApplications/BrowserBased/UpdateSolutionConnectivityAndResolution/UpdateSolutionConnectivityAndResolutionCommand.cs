using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public IUpdateBrowserBasedConnectivityAndResolutionData Data { get; }

        public UpdateSolutionConnectivityAndResolutionCommand(string id, IUpdateBrowserBasedConnectivityAndResolutionData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

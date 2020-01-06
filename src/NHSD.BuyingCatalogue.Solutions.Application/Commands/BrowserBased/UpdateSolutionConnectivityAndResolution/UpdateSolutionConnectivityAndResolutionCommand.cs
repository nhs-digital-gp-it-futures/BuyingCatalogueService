using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public UpdateSolutionConnectivityAndResolutionViewModel Data { get; }

        public UpdateSolutionConnectivityAndResolutionCommand(string id, UpdateSolutionConnectivityAndResolutionViewModel data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.MinimumConnectionSpeed = Data.MinimumConnectionSpeed?.Trim();
            Data.MinimumDesktopResolution = Data.MinimumDesktopResolution?.Trim();
        }
    }
}

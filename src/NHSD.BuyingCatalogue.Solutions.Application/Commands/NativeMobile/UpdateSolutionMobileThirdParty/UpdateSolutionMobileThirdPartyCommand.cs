using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileThirdParty
{
    public class UpdateSolutionMobileThirdPartyCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }

        public UpdateSolutionMobileThirdPartyViewModel Data { get; }

        public UpdateSolutionMobileThirdPartyCommand(string id, UpdateSolutionMobileThirdPartyViewModel data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.DeviceCapabilities = Data.DeviceCapabilities?.Trim();
            Data.ThirdPartyComponents = Data.ThirdPartyComponents?.Trim();
        }
    }
}

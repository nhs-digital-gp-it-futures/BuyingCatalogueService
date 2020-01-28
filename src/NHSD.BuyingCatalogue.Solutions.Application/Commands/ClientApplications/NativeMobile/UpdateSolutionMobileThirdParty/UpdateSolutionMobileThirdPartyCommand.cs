using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty
{
    public class UpdateSolutionMobileThirdPartyCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }

        public IUpdateNativeMobileThirdPartyData Data { get; }

        public UpdateSolutionMobileThirdPartyCommand(string id, IUpdateNativeMobileThirdPartyData data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

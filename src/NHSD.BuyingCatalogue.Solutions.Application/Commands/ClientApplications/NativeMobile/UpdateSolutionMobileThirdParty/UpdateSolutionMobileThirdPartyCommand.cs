using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty
{
    public sealed class UpdateSolutionMobileThirdPartyCommand : IRequest<ISimpleResult>
    {
        public UpdateSolutionMobileThirdPartyCommand(string id, IUpdateNativeMobileThirdPartyData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string Id { get; }

        public IUpdateNativeMobileThirdPartyData Data { get; }
    }
}

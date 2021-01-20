using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty
{
    public sealed class UpdateSolutionNativeDesktopThirdPartyCommand : IRequest<ISimpleResult>
    {
        public UpdateSolutionNativeDesktopThirdPartyCommand(string solutionId, IUpdateNativeDesktopThirdPartyData data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string SolutionId { get; }

        public IUpdateNativeDesktopThirdPartyData Data { get; }
    }
}

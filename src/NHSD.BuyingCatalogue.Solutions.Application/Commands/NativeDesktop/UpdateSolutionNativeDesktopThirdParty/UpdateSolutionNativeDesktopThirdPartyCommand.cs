using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty
{
    public sealed class UpdateSolutionNativeDesktopThirdPartyCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdateNativeDesktopThirdPartyData Data { get; }

        public UpdateSolutionNativeDesktopThirdPartyCommand(string solutionId, IUpdateNativeDesktopThirdPartyData data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data = data?.Trim();
        }
    }
}

using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionConnectivityDetails
{
    public sealed class UpdateSolutionNativeDesktopConnectivityDetailsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string NativeDesktopMinimumConnectionSpeed { get; }

        public UpdateSolutionNativeDesktopConnectivityDetailsCommand(string solutionId, string nativeDesktopMinimumConnectionSpeed)
        {
            SolutionId = solutionId.ThrowIfNull();
            NativeDesktopMinimumConnectionSpeed = nativeDesktopMinimumConnectionSpeed;
        }
    }
}

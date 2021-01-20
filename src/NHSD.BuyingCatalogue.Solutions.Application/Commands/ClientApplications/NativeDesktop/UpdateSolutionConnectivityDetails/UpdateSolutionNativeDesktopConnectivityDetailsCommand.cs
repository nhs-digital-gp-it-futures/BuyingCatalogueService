using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails
{
    public sealed class UpdateSolutionNativeDesktopConnectivityDetailsCommand : IRequest<ISimpleResult>
    {
        public UpdateSolutionNativeDesktopConnectivityDetailsCommand(string solutionId, string nativeDesktopMinimumConnectionSpeed)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            NativeDesktopMinimumConnectionSpeed = nativeDesktopMinimumConnectionSpeed;
        }

        public string SolutionId { get; }

        public string NativeDesktopMinimumConnectionSpeed { get; }
    }
}

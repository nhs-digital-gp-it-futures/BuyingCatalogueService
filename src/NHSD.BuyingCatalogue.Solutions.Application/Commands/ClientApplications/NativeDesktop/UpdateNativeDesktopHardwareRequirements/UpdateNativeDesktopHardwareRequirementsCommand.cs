using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements
{
    public sealed class UpdateNativeDesktopHardwareRequirementsCommand : IRequest<ISimpleResult>
    {
        public UpdateNativeDesktopHardwareRequirementsCommand(string solutionId, string hardwareRequirements)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            HardwareRequirements = hardwareRequirements;
        }

        public string SolutionId { get; }

        public string HardwareRequirements { get; }
    }
}

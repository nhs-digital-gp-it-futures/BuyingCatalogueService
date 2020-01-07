using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopHardwareRequirements
{
    public sealed class UpdateNativeDesktopHardwareRequirementsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string HardwareRequirements { get; }

        public UpdateNativeDesktopHardwareRequirementsCommand(string solutionId, string hardwareRequirements)
        {
            SolutionId = solutionId.ThrowIfNull();
            HardwareRequirements = hardwareRequirements?.Trim();
        }
    }
}

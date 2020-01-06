using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileHardwareRequirements
{
    public sealed class UpdateSolutionNativeMobileHardwareRequirementsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string HardwareRequirements { get; }

        public UpdateSolutionNativeMobileHardwareRequirementsCommand(string solutionId, string hardwareRequirements)
        {
            SolutionId = solutionId.ThrowIfNull();
            HardwareRequirements = hardwareRequirements?.Trim();
        }
    }
}

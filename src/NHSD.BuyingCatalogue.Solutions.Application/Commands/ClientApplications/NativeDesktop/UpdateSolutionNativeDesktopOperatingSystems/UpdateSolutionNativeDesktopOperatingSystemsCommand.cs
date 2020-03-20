using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems
{
    public sealed class UpdateSolutionNativeDesktopOperatingSystemsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string NativeDesktopOperatingSystemsDescription { get; }

        public UpdateSolutionNativeDesktopOperatingSystemsCommand(string solutionId, string nativeDesktopOperatingSystemsDescription)
        {
            SolutionId = solutionId.ThrowIfNull();
            NativeDesktopOperatingSystemsDescription = nativeDesktopOperatingSystemsDescription;
        }
    }
}

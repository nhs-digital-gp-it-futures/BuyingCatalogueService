using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst
{
    public sealed class UpdateSolutionNativeMobileFirstCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string MobileFirstDesign { get; }

        public UpdateSolutionNativeMobileFirstCommand(string solutionId, string mobileFirstDesign)
        {
            SolutionId = solutionId.ThrowIfNull();
            MobileFirstDesign = mobileFirstDesign;
        }
    }
}

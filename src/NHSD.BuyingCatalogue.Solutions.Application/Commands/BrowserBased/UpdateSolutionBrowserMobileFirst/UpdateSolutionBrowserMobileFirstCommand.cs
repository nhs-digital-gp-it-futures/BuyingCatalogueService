using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserMobileFirst
{
    public sealed class UpdateSolutionBrowserMobileFirstCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string MobileFirstDesign { get; }

        public UpdateSolutionBrowserMobileFirstCommand(string solutionId, string mobileFirstDesign)
        {
            SolutionId = solutionId.ThrowIfNull();
            MobileFirstDesign = mobileFirstDesign;
        }
    }
}

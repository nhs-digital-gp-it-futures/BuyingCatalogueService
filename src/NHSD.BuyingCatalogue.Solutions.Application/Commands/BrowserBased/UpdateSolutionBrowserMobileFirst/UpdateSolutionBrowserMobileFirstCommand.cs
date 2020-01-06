using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserMobileFirst
{
    public sealed class UpdateSolutionBrowserMobileFirstCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionBrowserMobileFirstViewModel Data { get; }

        public UpdateSolutionBrowserMobileFirstCommand(string solutionId, UpdateSolutionBrowserMobileFirstViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.MobileFirstDesign = Data.MobileFirstDesign?.Trim();
        }
    }
}

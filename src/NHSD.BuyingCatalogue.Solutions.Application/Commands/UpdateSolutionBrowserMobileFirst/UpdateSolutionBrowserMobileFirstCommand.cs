using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    public sealed class UpdateSolutionBrowserMobileFirstCommand : IRequest<UpdateSolutionBrowserMobileFirstValidationResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionBrowserMobileFirstViewModel UpdateSolutionBrowserMobileFirstViewModel { get; }

        public UpdateSolutionBrowserMobileFirstCommand(string solutionId, UpdateSolutionBrowserMobileFirstViewModel updateSolutionBrowserMobileFirstViewModel)
        {
            SolutionId = solutionId.ThrowIfNull();
            UpdateSolutionBrowserMobileFirstViewModel = updateSolutionBrowserMobileFirstViewModel.ThrowIfNull();
        }
    }
}

using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileFirst
{
    public sealed class UpdateSolutionNativeMobileFirstCommand : IRequest<RequiredResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionNativeMobileFirstViewModel UpdateSolutionNativeMobileFirstViewModel { get; }

        public UpdateSolutionNativeMobileFirstCommand(string solutionId, UpdateSolutionNativeMobileFirstViewModel updateSolutionNativeMobileFirstViewModel)
        {
            SolutionId = solutionId.ThrowIfNull();
            UpdateSolutionNativeMobileFirstViewModel = updateSolutionNativeMobileFirstViewModel.ThrowIfNull();
        }
    }
}

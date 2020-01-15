using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileFirst
{
    public sealed class UpdateSolutionNativeMobileFirstCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionNativeMobileFirstViewModel Data { get; }

        public UpdateSolutionNativeMobileFirstCommand(string solutionId, UpdateSolutionNativeMobileFirstViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

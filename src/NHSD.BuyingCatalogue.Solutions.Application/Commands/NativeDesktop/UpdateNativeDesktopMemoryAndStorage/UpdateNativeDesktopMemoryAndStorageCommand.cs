using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopMemoryAndStorage
{
    public sealed class UpdateNativeDesktopMemoryAndStorageCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdateNativeDesktopMemoryAndStorageData Data { get; }

        public UpdateNativeDesktopMemoryAndStorageCommand(string solutionId, IUpdateNativeDesktopMemoryAndStorageData data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull().Trim();
        }
    }
}

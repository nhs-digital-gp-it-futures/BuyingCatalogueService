using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage
{
    public sealed class UpdateNativeDesktopMemoryAndStorageCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdateNativeDesktopMemoryAndStorageData Data { get; }

        public UpdateNativeDesktopMemoryAndStorageCommand(string solutionId, IUpdateNativeDesktopMemoryAndStorageData data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

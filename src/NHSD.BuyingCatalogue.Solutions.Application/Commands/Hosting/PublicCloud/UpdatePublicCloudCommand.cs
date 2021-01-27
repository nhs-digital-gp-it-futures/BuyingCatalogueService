using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.PublicCloud
{
    public sealed class UpdatePublicCloudCommand : IRequest<ISimpleResult>
    {
        public UpdatePublicCloudCommand(string solutionId, IUpdatePublicCloudData data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string SolutionId { get; }

        public IUpdatePublicCloudData Data { get; }
    }
}

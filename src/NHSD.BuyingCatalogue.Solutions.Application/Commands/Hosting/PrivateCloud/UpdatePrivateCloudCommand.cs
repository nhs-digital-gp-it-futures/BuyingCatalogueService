using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.PrivateCloud
{
    public sealed class UpdatePrivateCloudCommand : IRequest<ISimpleResult>
    {
        public UpdatePrivateCloudCommand(string id, IUpdatePrivateCloudData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string Id { get; }

        public IUpdatePrivateCloudData Data { get; }
    }
}

using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud
{
    public sealed class UpdatePrivateCloudCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }

        public IUpdatePrivateCloudData Data { get; }

        public UpdatePrivateCloudCommand(string id, IUpdatePrivateCloudData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

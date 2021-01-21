using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.HybridHostingType
{
    public sealed class UpdateHybridHostingTypeCommand : IRequest<ISimpleResult>
    {
        public UpdateHybridHostingTypeCommand(string id, IUpdateHybridHostingTypeData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string Id { get; }

        public IUpdateHybridHostingTypeData Data { get; }
    }
}

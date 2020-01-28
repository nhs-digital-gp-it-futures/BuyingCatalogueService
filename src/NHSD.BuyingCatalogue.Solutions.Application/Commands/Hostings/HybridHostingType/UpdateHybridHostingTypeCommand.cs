using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.HybridHostingType
{
    public sealed class UpdateHybridHostingTypeCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }

        public IUpdateHybridHostingTypeData Data { get; }

        public UpdateHybridHostingTypeCommand(string id, IUpdateHybridHostingTypeData data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

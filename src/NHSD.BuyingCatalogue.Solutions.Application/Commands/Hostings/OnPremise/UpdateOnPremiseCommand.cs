using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise
{
    public sealed class UpdateOnPremiseCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }

        public IUpdateOnPremiseData Data { get; }

        public UpdateOnPremiseCommand(string id, IUpdateOnPremiseData data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

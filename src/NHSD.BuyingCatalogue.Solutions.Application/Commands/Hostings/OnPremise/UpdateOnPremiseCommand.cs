using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise
{
    public sealed class UpdateOnPremiseCommand : IRequest<ISimpleResult>
    {
        public UpdateOnPremiseCommand(string id, IUpdateOnPremiseData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string Id { get; }

        public IUpdateOnPremiseData Data { get; }
    }
}

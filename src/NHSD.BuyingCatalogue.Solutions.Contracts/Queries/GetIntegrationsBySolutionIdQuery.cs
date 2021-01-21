using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetIntegrationsBySolutionIdQuery : IRequest<IIntegrations>
    {
        public GetIntegrationsBySolutionIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}

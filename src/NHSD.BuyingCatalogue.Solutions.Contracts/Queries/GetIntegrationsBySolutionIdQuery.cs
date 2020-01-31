using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetIntegrationsBySolutionIdQuery : IRequest<IIntegrations>
    {
        public string Id { get; }

        public GetIntegrationsBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

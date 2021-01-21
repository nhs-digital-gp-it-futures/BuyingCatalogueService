using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetImplementationTimescalesBySolutionIdQuery : IRequest<IImplementationTimescales>
    {
        public GetImplementationTimescalesBySolutionIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}

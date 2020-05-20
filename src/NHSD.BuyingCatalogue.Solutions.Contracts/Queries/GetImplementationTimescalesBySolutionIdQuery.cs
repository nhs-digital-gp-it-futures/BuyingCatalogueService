using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetImplementationTimescalesBySolutionIdQuery : IRequest<IImplementationTimescales>
    {
        public string Id { get; }

        public GetImplementationTimescalesBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

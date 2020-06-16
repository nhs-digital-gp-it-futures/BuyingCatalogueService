using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetRoadMapBySolutionIdQuery : IRequest<IRoadMap>
    {
        public string Id { get; }

        public GetRoadMapBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

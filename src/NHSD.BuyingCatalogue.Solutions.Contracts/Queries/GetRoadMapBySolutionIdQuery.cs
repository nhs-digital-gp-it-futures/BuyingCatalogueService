using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetRoadMapBySolutionIdQuery : IRequest<IRoadMap>
    {
        public GetRoadMapBySolutionIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}

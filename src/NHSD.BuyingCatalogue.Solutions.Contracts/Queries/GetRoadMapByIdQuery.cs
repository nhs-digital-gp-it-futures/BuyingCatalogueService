using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetRoadMapByIdQuery : IRequest<string>
    {
        public string Id { get; }

        public GetRoadMapByIdQuery(string id)
        {
            Id = id;
        }
    }
}

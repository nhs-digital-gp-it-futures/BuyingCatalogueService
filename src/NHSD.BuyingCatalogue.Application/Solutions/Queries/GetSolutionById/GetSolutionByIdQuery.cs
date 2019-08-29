using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    public sealed class GetSolutionByIdQuery : IRequest<GetSolutionByIdResult>
    {
        public string Id { get; }

        public GetSolutionByIdQuery(string id)
        {
            Id = id;
        }
    }
}

using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    /// <summary>
    /// Represents the query parameters for the get Solution by ID request.
    /// </summary>
    public sealed class GetSolutionByIdQuery : IRequest<ISolution>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSolutionByIdQuery"/> class.
        /// </summary>
        /// <param name="id">The ID of the solution.</param>
        public GetSolutionByIdQuery(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the key information to identify a solution.
        /// </summary>
        public string Id { get; }
    }
}

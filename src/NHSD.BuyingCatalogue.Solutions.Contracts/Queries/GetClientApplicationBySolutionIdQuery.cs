using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    /// <summary>
    /// Represents the query parameters for the get client application by solution ID request.
    /// </summary>
    public sealed class GetClientApplicationBySolutionIdQuery : IRequest<IClientApplication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetClientApplicationBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id">The ID of the solution.</param>
        public GetClientApplicationBySolutionIdQuery(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the key information to identify a solution.
        /// </summary>
        public string Id { get; }
    }
}

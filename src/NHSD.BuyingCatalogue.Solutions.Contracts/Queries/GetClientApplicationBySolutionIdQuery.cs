using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    /// <summary>
    /// Represents the query parameters for the get client application by solution ID request
    /// </summary>
    public sealed class GetClientApplicationBySolutionIdQuery : IRequest<IClientApplication>
    {
        /// <summary>
        /// The key information to identify a <see cref="Solution"/>
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetClientApplicationBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id"></param>
        public GetClientApplicationBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

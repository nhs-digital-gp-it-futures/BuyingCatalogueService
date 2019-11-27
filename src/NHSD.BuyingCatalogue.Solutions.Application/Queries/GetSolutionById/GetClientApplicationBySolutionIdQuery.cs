using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
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

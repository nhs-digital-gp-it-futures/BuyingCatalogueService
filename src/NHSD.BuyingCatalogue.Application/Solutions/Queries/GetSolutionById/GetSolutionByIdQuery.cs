using MediatR;
using Microsoft.AspNetCore.Http;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// Represents the query paramters for the get Solution by ID request.
    /// </summary>
    public sealed class GetSolutionByIdQuery : IRequest<GetSolutionByIdResult>
    {
        public IIdentityProvider IdProvider { get; }

        /// <summary>
        /// The key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSolutionByIdQuery"/> class.
        /// </summary>
        public GetSolutionByIdQuery(IIdentityProvider idProvider, string id)
        {
            IdProvider = idProvider;
            Id = id;
        }
    }
}

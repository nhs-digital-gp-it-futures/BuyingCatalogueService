using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes
{
    /// <summary>
    /// Represents the query paramters for the Client Application Types request.
    /// </summary>
    public sealed class GetClientApplicationTypesQuery : IRequest<GetClientApplicationTypesResult>
    {
        /// <summary>
        /// The key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetClientApplicationTypes.GetClientApplicationTypesQuery"/> class.
        /// </summary>
        public GetClientApplicationTypesQuery(string id)
        {
            Id = id;
        }
    }
}

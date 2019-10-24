using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetBrowsersSupported
{
    /// <summary>
    /// Represents the query paramters for the Browsers Supported request.
    /// </summary>
    public sealed class GetBrowsersSupportedQuery : IRequest<GetBrowsersSupportedResult>
    {
        /// <summary>
        /// The key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetBrowsersSupported.GetBrowsersSupportedQuery"/> class.
        /// </summary>
        public GetBrowsersSupportedQuery(string id)
        {
            Id = id;
        }
    }
}

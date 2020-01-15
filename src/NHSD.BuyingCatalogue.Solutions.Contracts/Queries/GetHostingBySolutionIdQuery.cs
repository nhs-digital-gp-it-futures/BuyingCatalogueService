using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetHostingBySolutionIdQuery : IRequest<IHosting>
    {
        /// <summary>
        /// The Id of the <see cref="Solution"/> to retrieve Hosting information for
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetHostingBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id">The ID of the Solution to retrieve the Hosting for</param>
        public GetHostingBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

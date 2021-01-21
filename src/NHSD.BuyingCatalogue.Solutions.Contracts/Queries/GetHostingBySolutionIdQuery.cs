using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetHostingBySolutionIdQuery : IRequest<IHosting>
    {
        /// <summary>
        /// Gets the ID of the solution to retrieve Hosting information for.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHostingBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id">The ID of the Solution to retrieve the Hosting for.</param>
        public GetHostingBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

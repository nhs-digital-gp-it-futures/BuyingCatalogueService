using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetSupplierBySolutionIdQuery : IRequest<ISupplier>
    {
        /// <summary>
        /// The Id of the <see cref="Solution"/> to retrieve AboutSupplier for
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSupplierBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id">The ID of the Solution to retrieve the AboutSupplier</param>
        public GetSupplierBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}

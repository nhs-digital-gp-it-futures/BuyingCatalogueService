using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetSupplierBySolutionIdQuery : IRequest<ISupplier>
    {
        /// <summary>
        /// The Id of the <see cref="Solution"/> to retrieve Supplier for
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSupplierBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id">The ID of the Solution to retrieve the Supplier</param>
        public GetSupplierBySolutionIdQuery(string solutionId)
        {
            SolutionId = solutionId;
        }
    }
}

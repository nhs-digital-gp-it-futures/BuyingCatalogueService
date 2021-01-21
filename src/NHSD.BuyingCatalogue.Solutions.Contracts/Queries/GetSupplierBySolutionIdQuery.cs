using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetSupplierBySolutionIdQuery : IRequest<ISolutionSupplier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSupplierBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="solutionId">The ID of the solution to retrieve the supplier.</param>
        public GetSupplierBySolutionIdQuery(string solutionId)
        {
            SolutionId = solutionId;
        }

        /// <summary>
        /// Gets the ID of the solution to retrieve the supplier for.
        /// </summary>
        public string SolutionId { get; }
    }
}

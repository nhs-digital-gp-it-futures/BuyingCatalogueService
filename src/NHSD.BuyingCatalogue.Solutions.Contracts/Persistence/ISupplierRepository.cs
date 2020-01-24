using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierRepository
    {
        /// <summary>
        /// Retrieve the Suppliers for a solution
        /// </summary>
        /// <param name="solutionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task containing the supplier data</returns>
        Task<ISupplierResult> GetSupplierBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the Suppliers for a solution
        /// </summary>
        /// <param name="updateSupplierRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task containing the supplier data</returns>
        Task UpdateSupplierAsync(IUpdateSupplierRequest updateSupplierRequest, CancellationToken cancellationToken);
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the
    /// <see cref="ISolutionListResult"/> object.
    /// </summary>
    public interface ISolutionListRepository
    {
        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <param name="foundationOnly">Specify <see langword="true"/> to include foundation solutions only.</param>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        Task<IEnumerable<ISolutionListResult>> ListAsync(
            bool foundationOnly,
            string supplierId,
            CancellationToken cancellationToken);
    }
}

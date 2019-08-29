using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Domain;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the <see cref="Solution"/> domain.
    /// </summary>
    public interface ISolutionRepository
    {
        /// <summary>
        /// Gets a list of <see cref="Solution"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="Solution"/> objects.</returns>
        Task<IEnumerable<Solution>> ListSolutionSummaryAsync(ISet<string> capabilityIdList, CancellationToken cancellationToken);

        Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken);
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the <see cref="ICapabilityListResult"/> object.
    /// </summary>
    public interface ICapabilityRepository
	{
		/// <summary>
		/// Gets a list of <see cref="ICapabilityListResult"/> objects.
		/// </summary>
		/// <returns>A list of <see cref="ICapabilityListResult"/> objects.</returns>
		Task<IEnumerable<ICapabilityListResult>> ListAsync(CancellationToken cancellationToken);
	}
}

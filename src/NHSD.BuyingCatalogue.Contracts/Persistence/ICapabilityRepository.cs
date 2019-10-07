using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the <see cref="Capability"/> domain.
    /// </summary>
    public interface ICapabilityRepository
	{
		/// <summary>
		/// Gets a list of <see cref="Capability"/> objects.
		/// </summary>
		/// <returns>A list of <see cref="Capability"/> objects.</returns>
		Task<IEnumerable<ICapabilityListResult>> ListAsync(CancellationToken cancellationToken);
	}
}

﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the
    /// <see cref="ICapabilityListResult"/> object.
    /// </summary>
    public interface ICapabilityRepository
    {
        /// <summary>
        /// Gets a list of <see cref="ICapabilityListResult"/> objects.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A list of <see cref="ICapabilityListResult"/> objects.</returns>
        Task<IEnumerable<ICapabilityListResult>> ListAsync(CancellationToken cancellationToken);
    }
}

using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    /// <summary>
    /// Defines the data contract representing a factory to provide a new connection to the data store.
    /// </summary>
    internal interface IDbConnectionFactory
    {
        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A task representing an operation to create a new database connection.</returns>
        Task<IDbConnection> GetAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <param name="connectionStringBuilder">A connection string builder.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A new database connection.</returns>
        Task<IDbConnection> GetAsync(DbConnectionStringBuilder connectionStringBuilder, CancellationToken cancellationToken);
    }
}

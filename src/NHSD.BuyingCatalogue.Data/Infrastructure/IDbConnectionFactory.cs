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
        /// <returns>A task representing an operation to create a new database connection.</returns>
        Task<IDbConnection> GetAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        Task<IDbConnection> GetAsync(CancellationToken cancellationToken, DbConnectionStringBuilder connectionStringBuilder);
    }
}

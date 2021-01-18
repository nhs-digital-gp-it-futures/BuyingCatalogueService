using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    /// <summary>
    /// A factory to provide a new database connection.
    /// </summary>
    internal sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        /// <param name="settings">The settings containing the connection string.</param>
        public DbConnectionFactory(ISettings settings) =>
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A new database connection.</returns>
        public async Task<IDbConnection> GetAsync(CancellationToken cancellationToken)
            => await GetAsync(new SqlConnectionStringBuilder(settings.ConnectionString), cancellationToken);

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <param name="connectionStringBuilder">A connection string builder.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A new database connection.</returns>
        public async Task<IDbConnection> GetAsync(
            DbConnectionStringBuilder connectionStringBuilder,
            CancellationToken cancellationToken)
        {
            var connection = SqlClientFactory.Instance.CreateConnection();

            if (connectionStringBuilder is null)
            {
                throw new ArgumentNullException(nameof(connectionStringBuilder));
            }

            // ReSharper disable once PossibleNullReferenceException
            // (SqlClientFactory.CreateConnection should throw PlatformNotSupportedException)
            connection.ConnectionString = connectionStringBuilder.ConnectionString;

            if (connection is SqlConnection sqlConnection)
            {
                sqlConnection.StatisticsEnabled = true;
            }

            await connection.OpenAsync(cancellationToken);

            return connection;
        }
    }
}

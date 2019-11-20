using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Persistence.Infrastructure
{
    /// <summary>
    /// A factory to provide a new database connection.
    /// </summary>
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Gets the database connection details.
        /// </summary>
        private string DefaultConnectionString => _configuration.BuyingCatalogueConnectionString();

        /// <summary>
        /// Initialises a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        public DbConnectionFactory(IConfiguration configuration)
            => _configuration = configuration.ThrowIfNull(nameof(configuration));

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        public async Task<IDbConnection> GetAsync(CancellationToken cancellationToken)
            => await GetAsync(cancellationToken, new SqlConnectionStringBuilder(DefaultConnectionString));

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        public async Task<IDbConnection> GetAsync(CancellationToken cancellationToken, DbConnectionStringBuilder connectionStringBuilder)
        {
            var connection = SqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionStringBuilder.ThrowIfNull().ConnectionString;

            await connection.OpenAsync(cancellationToken);

            return connection;
        }
    }
}

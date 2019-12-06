using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Data.HealthChecks
{
    internal class RepositoryHealthCheck : IRepositoryHealthCheck
    {
        private const string Sql = "SELECT 1";
        private static readonly int ConnectTimeout = (int)TimeSpan.FromSeconds(5).TotalSeconds;

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ISettings _configuration;

        /// <summary>
        /// Initialises a new instance of the <see cref="RepositoryHealthCheck"/> class.
        /// </summary>
        public RepositoryHealthCheck(IDbConnectionFactory dbConnectionFactory, ISettings configuration)
        {
            _dbConnectionFactory = dbConnectionFactory.ThrowIfNull(nameof(dbConnectionFactory));
            _configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        /// <summary>
        /// Activates the status check of the repository.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the action.</param>
        /// <returns>A task representing an operation to check the health of the repository.</returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_configuration.ConnectionString)
            {
                ConnectTimeout = ConnectTimeout
            };

            using var dbConnection = await _dbConnectionFactory.GetAsync(sqlConnectionStringBuilder, cancellationToken).ConfigureAwait(false);
            var result = await dbConnection.QuerySingleAsync<int>(Sql).ConfigureAwait(false);
        }
    }
}

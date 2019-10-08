using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;

namespace NHSD.BuyingCatalogue.Persistence.HealthChecks
{
    public class RepositoryHealthCheck : IRepositoryHealthCheck
    {
        private const string Sql = "SELECT 1";
        private static readonly int ConnectTimeout = (int)TimeSpan.FromSeconds(5).TotalSeconds;

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialises a new instance of the <see cref="RepositoryHealthCheck"/> class.
        /// </summary>
        public RepositoryHealthCheck(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Activates the status check of the repository.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the action.</param>
        /// <returns>A task representing an operation to check the health of the repository.</returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_configuration.BuyingCatalogueConnectionString());
            sqlConnectionStringBuilder.ConnectTimeout = ConnectTimeout;

            using (var dbConnection = await _dbConnectionFactory.GetAsync(cancellationToken, sqlConnectionStringBuilder))
            {
                var result = await dbConnection.QuerySingleAsync<int>(Sql);
            }
        }
    }
}

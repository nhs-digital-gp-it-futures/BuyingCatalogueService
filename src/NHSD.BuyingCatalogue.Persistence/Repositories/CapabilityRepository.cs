using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
	/// <summary>
	/// Represents the data access layer for the <see cref="Capability"/> entity.
	/// </summary>
	public sealed class CapabilityRepository : ICapabilityRepository
	{
		/// <summary>
		/// Database connection factory to provide new connections.
		/// </summary>
		public IDbConnectionFactory DbConnectionFactory { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="CapabilityRepository"/> class.
		/// </summary>
		public CapabilityRepository(IDbConnectionFactory dbConnectionFactory)
		{
			DbConnectionFactory = dbConnectionFactory ?? throw new System.ArgumentNullException(nameof(dbConnectionFactory));
		}

		/// <summary>
		/// Gets a list of <see cref="Capability"/> objects.
		/// </summary>
		/// <returns>A list of <see cref="Capability"/> objects.</returns>
		public async Task<IEnumerable<Capability>> ListAsync(CancellationToken cancellationToken)
		{
			using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
			{
				const string sql = @"SELECT  CONVERT(VARCHAR(36), Capability.Id) AS Id, 
											 Name, 
											 ISNULL(IsFoundation, 0) AS IsFoundation
									FROM	 Capability 
											 LEFT OUTER JOIN FrameworkCapabilities ON Capability.Id = FrameworkCapabilities.CapabilityId
                                    ORDER BY IsFoundation DESC, UPPER(Name) ASC";

				return (await databaseConnection.QueryAsync<Capability>(sql).ConfigureAwait(false)).ToList();
			}
		}
	}
}

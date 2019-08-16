using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain;
using NHSD.BuyingCatalogue.Domain.Entities;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using Dapper;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
	/// <summary>
	/// Represents the data access layer for the <see cref="Solution"/> entity.
	/// </summary>
	public sealed class SolutionRepository : ISolutionRepository
	{
		/// <summary>
		/// Database connection factory to provide new connections.
		/// </summary>
		public IDbConnectionFactory DbConnectionFactory { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="SolutionRepository"/> class.
		/// </summary>
		public SolutionRepository(IDbConnectionFactory dbConnectionFactory)
		{
			DbConnectionFactory = dbConnectionFactory ?? throw new System.ArgumentNullException(nameof(dbConnectionFactory));
		}

		/// <summary>
		/// Gets a list of <see cref="Solution"/> objects.
		/// </summary>
		/// <returns>A list of <see cref="Solution"/> objects.</returns>
		public async Task<IEnumerable<Solution>> ListSolutionSummaryAsync(CancellationToken cancellationToken)
		{
			using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
			{
				const string sql = @"SELECT	Solution.Id, 
											Solution.SolutionName as Name, 
											Solution.Summary, 
											Organisation.Id, 
											Organisation.OrganisationName AS Name,
											CONVERT(VARCHAR(36), Capability.Id) AS Id,
											Capability.CapabilityName AS Name,
											Capability.CapabilityDescription AS Description
									FROM	Solution 
											INNER JOIN Organisation ON Organisation.Id = Solution.OrganisationId
											INNER JOIN SolutionCapability ON Solution.Id = SolutionCapability.SolutionId
											INNER JOIN Capability ON Capability.Id = SolutionCapability.CapabilityId";

				Dictionary<string, Solution> solutionDictionary = new Dictionary<string, Solution>();

				IEnumerable<Solution> result = await databaseConnection.QueryAsync<Solution, Organisation, Capability, Solution>(sql, (solution, organisation, capability) =>
				{
					string solutionId = solution.Id;
					if (!solutionDictionary.TryGetValue(solutionId, out Solution currentSolution))
					{
						solution.Organisation = organisation;
						solutionDictionary.Add(solutionId, solution);

						currentSolution = solution;
					}

					currentSolution.AddCapability(capability);

					return currentSolution;
				});

				return result.Distinct().AsList();
			}
		}
	}
}

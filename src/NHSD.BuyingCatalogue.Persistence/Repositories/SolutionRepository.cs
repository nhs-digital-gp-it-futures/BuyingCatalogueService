using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain;
using NHSD.BuyingCatalogue.Domain.Entities;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;

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
        private IDbConnectionFactory DbConnectionFactory { get; }

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
        public async Task<IEnumerable<Solution>> ListAsync(ISet<string> capabilityIdList, CancellationToken cancellationToken)
		{
            if (capabilityIdList is null)
            {
                throw new System.ArgumentNullException(nameof(capabilityIdList));
            }

            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT Solution.Id, 
                                            Solution.Name, 
                                            Solution.Summary, 
                                            Organisation.Id, 
                                            Organisation.Name,
                                            CONVERT(VARCHAR(36), Capability.Id) AS Id,
                                            Capability.Name,
                                            Capability.Description
                                    FROM    Solution 
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

                IEnumerable<Solution> solutionList = result.Distinct();
                if (capabilityIdList.Any())
                {
                    solutionList = solutionList.Where(solution => capabilityIdList.Intersect(solution.Capabilities.Select(capability => capability.Id)).Count() == capabilityIdList.Count());
                }

                return solutionList;
            }
        }

        /// <summary>
        /// Gets a <see cref="Solution"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of a <see cref="Solution"/>.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="Solution"/> matching the specified ID.</returns>
        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT Solution.Id,
                                            Solution.Name,
                                            MarketingDetail.Features As Features
                                     FROM   Solution
                                            INNER JOIN MarketingDetail ON Solution.Id = MarketingDetail.SolutionId
                                     WHERE  Solution.Id = @id";

                var result = await databaseConnection.QueryAsync<Solution>(sql, new { id });

                return result.SingleOrDefault();
            }
        }

        /// <summary>
        /// Updates the details of the solution.
        /// </summary>
        /// <param name="solution">The updated details of a solution to save to the data store.</param>
        /// <returns>A task representing an operation to save the specified solution to the data store.</returns>
        public async Task UpdateAsync(Solution solution, CancellationToken cancellationToken)
        {
            if (solution is null)
            {
                throw new System.ArgumentNullException(nameof(solution));
            }

            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                using (IDbTransaction transaction = databaseConnection.BeginTransaction())
                {
                    const string updateMarketingDetailSql = @"
                                         UPDATE  MarketingDetail
                                         SET     MarketingDetail.Features = @features
                                         WHERE   MarketingDetail.SolutionId = @solutionId;";

                    await databaseConnection.ExecuteAsync(updateMarketingDetailSql, new { solutionId = solution.Id, features = solution.Features }, transaction);

                    transaction.Commit();
                }
            }
        }
    }
}

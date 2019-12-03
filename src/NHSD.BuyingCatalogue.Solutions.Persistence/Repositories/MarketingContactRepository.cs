using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class MarketingContactRepository : IMarketingContactRepository
    {
        /// <summary>
        /// Database connection factory to provide new connections.
        /// </summary>
        private readonly IDbConnector _dbConnector;

        public MarketingContactRepository(IDbConnector dbConnector)
        => _dbConnector = dbConnector ?? throw new System.ArgumentNullException(nameof(dbConnector));

        private const string getSql = @"SELECT 
                                    MarketingContact.Id
                                    ,MarketingContact.SolutionId
                                    ,MarketingContact.FirstName
                                    ,MarketingContact.LastName
                                    ,MarketingContact.Email
                                    ,MarketingContact.PhoneNumber
                                    ,MarketingContact.Department
                                    ,MarketingContact.LastUpdated
                                    FROM Solution
                                    INNER JOIN MarketingContact ON MarketingContact.SolutionId = Solution.Id
                                    WHERE Solution.Id = @solutionId";

        private const string deleteSql = @"DELETE FROM MarketingContact Where SolutionId = @solutionId";

        private const string insertSql = @"INSERT INTO [dbo].[MarketingContact]
            ([SolutionId]
            ,[FirstName]
            ,[LastName]
            ,[Email]
            ,[PhoneNumber]
            ,[Department]
            ,[LastUpdated]
            ,[LastUpdatedBy])
            VALUES(
            @solutionId,
            @firstName,
            @lastName,
            @email,
            @phoneNumber,
            @department,
            @lastUpdated,
            @lastUpdatedBy)";

        public async Task<IEnumerable<IMarketingContactResult>> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
                => await _dbConnector.QueryAsync<MarketingContactResult>(cancellationToken, getSql, new { solutionId });

        public async Task ReplaceContactsForSolution(string solutionId, IEnumerable<IContact> newContacts, CancellationToken cancellationToken)
        {
            var queries = new List<(string, object)> {(deleteSql, new {solutionId})};

            queries.AddRange(newContacts.Select(contact =>
                (insertSql,
                    (object)new
                    {
                        solutionId = solutionId,
                        firstName = contact.FirstName,
                        lastName = contact.LastName,
                        email = contact.Email,
                        phoneNumber = contact.PhoneNumber,
                        department = contact.Department,
                        lastUpdated = DateTime.Now,
                        lastUpdatedBy = Guid.Empty
                    })));

            await _dbConnector.ExecuteMultipleWithTransactionAsync(cancellationToken, queries);
        }
    }
}

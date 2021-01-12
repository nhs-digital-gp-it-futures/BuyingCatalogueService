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
        => _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));

        private const string GetSql = @"SELECT m.Id,
       m.SolutionId,
       m.FirstName,
       m.LastName,
       m.Email,
       m.PhoneNumber,
       m.Department,
       m.LastUpdated
  FROM dbo.Solution AS s
       INNER JOIN MarketingContact AS m
               ON m.SolutionId = s.Id
WHERE s.Id = @solutionId;";

        private const string DeleteSql = @"DELETE FROM dbo.MarketingContact Where SolutionId = @solutionId;";

        private const string InsertSql = @"INSERT INTO dbo.MarketingContact(
            SolutionId,
            FirstName,
            LastName,
            Email,
            PhoneNumber,
            Department,
            LastUpdated,
            LastUpdatedBy)
    VALUES (@solutionId,
            @firstName,
            @lastName,
            @email,
            @phoneNumber,
            @department,
            GETUTCDATE(),
            @lastUpdatedBy);";

        public async Task<IEnumerable<IMarketingContactResult>> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
                => await _dbConnector.QueryAsync<MarketingContactResult>(GetSql, cancellationToken, new { solutionId });

        public async Task ReplaceContactsForSolution(string solutionId, IEnumerable<IContact> newContacts, CancellationToken cancellationToken)
        {
            var queries = new List<(string, object)> { (DeleteSql, new { solutionId }) };

            queries.AddRange(newContacts.Select(contact =>
                (insertSql: InsertSql,
                    (object)new
                    {
                        solutionId,
                        firstName = contact.FirstName,
                        lastName = contact.LastName,
                        email = contact.Email,
                        phoneNumber = contact.PhoneNumber,
                        department = contact.Department,
                        lastUpdatedBy = Guid.Empty,
                    })));

            await _dbConnector.ExecuteMultipleWithTransactionAsync(queries, cancellationToken);
        }
    }
}

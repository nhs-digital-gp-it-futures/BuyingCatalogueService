using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SupplierRepository : ISupplierRepository
    {
        private const string GetSupplierBySolutionIdSql = @"SELECT
                                    Solution.Id as SolutionId,
                                    Supplier.Name as Name,
                                    Supplier.Summary as Summary,
                                    Supplier.SupplierUrl as Url
                                 FROM Supplier
                                      LEFT JOIN Solution ON Supplier.Id = Solution.SupplierId
                                 WHERE  Solution.Id = @solutionId";

        private const string UpdateSupplierBySolutionIdSql = @"UPDATE
                                    Supplier
                                    SET
                                    Supplier.Summary = @Summary,
                                    Supplier.SupplierUrl = @SupplierUrl,
                                    Supplier.LastUpdated = GETDATE()
                                    FROM Supplier
                                        INNER JOIN Solution
                                        ON Supplier.Id = Solution.SupplierId
                                        WHERE Solution.Id = @solutionId";

        // A full-text index is typically a better way of implementing this kind of search. For example, the LIKE search below will
        // always perform an index scan. Given the expected number of suppliers I doubt this will be a problem, however.
        private const string GetSuppliersByNameSql = @"SELECT sup.Id, sup.[Name]
     FROM dbo.Supplier AS sup
		  INNER JOIN dbo.Solution AS sol
		  ON sol.SupplierId = sup.Id
		  INNER JOIN dbo.PublicationStatus AS ps
		  ON ps.Id = sol.PublishedStatusId
    WHERE sup.[Name] LIKE '%' + @name + '%'
	  AND ps.[Name] = COALESCE(NULLIF(@status, ''), ps.[Name])
 ORDER BY sup.[Name];";

        // This query is non-deterministic as there is currently no way to identify a primary contact
        // TODO: define means of identifying a primary contact
        private const string GetSupplierByIdSql = @"WITH SupplierDetails AS
(
    SELECT TOP (1) s.Id, s.[Name],
           JSON_VALUE(s.[Address], '$.line1') AS AddressLine1,
           JSON_VALUE(s.[Address], '$.line2') AS AddressLine2,
           JSON_VALUE(s.[Address], '$.line3') AS AddressLine3,
           JSON_VALUE(s.[Address], '$.line4') AS AddressLine4,
           JSON_VALUE(s.[Address], '$.line5') AS AddressLine5,
           JSON_VALUE(s.[Address], '$.city') AS Town,
           JSON_VALUE(s.[Address], '$.county') AS County,
           JSON_VALUE(s.[Address], '$.postcode') AS Postcode,
           JSON_VALUE(s.[Address], '$.country') AS Country,
           c.FirstName AS PrimaryContactFirstName,
           c.LastName AS PrimaryContactLastName,
           c.Email AS PrimaryContactEmailAddress,
           c.PhoneNumber AS PrimaryContactTelephone
      FROM dbo.Supplier AS s
           LEFT OUTER JOIN dbo.SupplierContact AS c
           ON c.SupplierId = s.Id
     WHERE s.Id = @id
 )
 SELECT Id, [Name],
        AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5,
        Town, County, Postcode, Country,
        PrimaryContactFirstName, PrimaryContactLastName,
        PrimaryContactEmailAddress, PrimaryContactTelephone,
        CASE
        WHEN NULLIF(COALESCE(AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5,
             Town, County, Postcode, Country), '') IS NULL THEN 0
        ELSE 1 END AS HasAddress,
        CASE
        WHEN NULLIF(COALESCE(PrimaryContactFirstName, PrimaryContactFirstName,
             PrimaryContactEmailAddress, PrimaryContactTelephone), '') IS NULL THEN 0
        ELSE 1 END AS HasContact
   FROM SupplierDetails;";

        private readonly IDbConnector _dbConnector;

        public SupplierRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        public async Task<ISolutionSupplierResult> GetSupplierBySolutionIdAsync(string solutionId, CancellationToken cancellationToken) =>
            (await _dbConnector
                .QueryAsync<SolutionSupplierResult>(GetSupplierBySolutionIdSql, cancellationToken, new { solutionId })
                .ConfigureAwait(false)).SingleOrDefault();

        public async Task<ISupplierResult> GetSupplierById(string id, CancellationToken cancellationToken) =>
            await _dbConnector.QueryFirstOrDefaultAsync<SupplierResult>(GetSupplierByIdSql, cancellationToken, new { id });

        public async Task<IEnumerable<ISupplierResult>> GetSuppliersByName(string name, string solutionPublicationStatus, CancellationToken cancellationToken) =>
            await _dbConnector.QueryAsync<SupplierResult>(
                GetSuppliersByNameSql,
                cancellationToken,
                new
                {
                    Name = name ?? string.Empty,
                    Status = solutionPublicationStatus?.Trim()
                });

        public async Task UpdateSupplierAsync(IUpdateSupplierRequest updateSupplierRequest, CancellationToken cancellationToken)
        {
            if (updateSupplierRequest == null)
            {
                throw new ArgumentNullException(nameof(updateSupplierRequest));
            }

            await _dbConnector.ExecuteAsync(UpdateSupplierBySolutionIdSql, cancellationToken,
                new
                {
                    solutionId = updateSupplierRequest.SolutionId,
                    summary = updateSupplierRequest.Description,
                    supplierUrl = updateSupplierRequest.Link
                }).ConfigureAwait(false);
        }
    }
}

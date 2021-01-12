using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SupplierRepository : ISupplierRepository
    {
        private const string GetSupplierBySolutionIdSql = @"SELECT ci.CatalogueItemId as SolutionId,
       s.[Name],
       s.Summary,
       s.SupplierUrl AS [Url]
  FROM dbo.Supplier AS s
       LEFT OUTER JOIN dbo.CatalogueItem AS ci
               ON s.Id = ci.SupplierId
 WHERE ci.CatalogueItemId = @solutionId;";

        private const string UpdateSupplierBySolutionIdSql = @"UPDATE s
   SET Summary = @Summary,
       SupplierUrl = @SupplierUrl,
       LastUpdated = GETUTCDATE()
  FROM dbo.Supplier AS s
       INNER JOIN dbo.CatalogueItem AS ci
       ON ci.SupplierId = s.Id
 WHERE ci.CatalogueItemId = @solutionId;";

        // A full-text index is typically a better way of implementing this kind of search although the requirements prevent its use.
        // The LIKE search below will always perform an index scan.
        // Given the expected number of suppliers I doubt this will be a problem, however.
        private const string GetSuppliersByNameSql = @"SELECT s.Id, s.[Name]
     FROM dbo.Supplier AS s
    WHERE s.[Name] LIKE '%' + @name + '%'
      AND EXISTS (
          SELECT *
            FROM dbo.CatalogueItem AS c
           WHERE c.SupplierId = s.Id
             AND c.PublishedStatusId = ISNULL(NULLIF(@statusId, ''), c.PublishedStatusId)
             AND c.CatalogueItemTypeId = ISNULL(NULLIF(@catalogueItemTypeId, ''), c.CatalogueItemTypeId))
 ORDER BY s.[Name];";

        // This query is non-deterministic as there is currently no way to identify a primary contact
        // TODO: define means of identifying a primary contact (task 7581)
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
                .QueryAsync<SolutionSupplierResult>(GetSupplierBySolutionIdSql, cancellationToken, new { solutionId }))
            .SingleOrDefault();

        public async Task<ISupplierResult> GetSupplierById(string id, CancellationToken cancellationToken) =>
            await _dbConnector.QueryFirstOrDefaultAsync<SupplierResult>(GetSupplierByIdSql, cancellationToken, new { id });

        public async Task<IEnumerable<ISupplierResult>> GetSuppliersByNameAsync(
            string name,
            PublishedStatus? solutionPublicationStatus,
            CatalogueItemType? catalogueItemType,
            CancellationToken cancellationToken)
        {
            var escapedName = name;
            if (!string.IsNullOrWhiteSpace(name))
            {
                var regex = new Regex(@"(\[|%|\+|\&|_|-)");
                escapedName = regex.Replace(name, "[$1]");
            }

            return await _dbConnector.QueryAsync<SupplierResult>(
                GetSuppliersByNameSql,
                cancellationToken,
                new
                {
                    Name = escapedName ?? string.Empty,
                    StatusId = (int?)solutionPublicationStatus,
                    CatalogueItemTypeId = (int?)catalogueItemType,
                });
        }

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
                    supplierUrl = updateSupplierRequest.Link,
                });
        }
    }
}

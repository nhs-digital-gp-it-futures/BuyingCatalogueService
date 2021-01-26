using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class EpicEntity : EntityBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Guid CapabilityId { get; set; }

        public string SourceUrl { get; set; }

        public int CompliancyLevelId { get; set; }

        public bool Active { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.Epic
            (
                Id,
                [Name],
                CapabilityId,
                SourceUrl,
                CompliancyLevelId,
                Active
            )
            VALUES
            (
                @Id,
                @Name,
                @CapabilityId,
                @SourceUrl,
                @CompliancyLevelId,
                @Active
            );";

        public static async Task<IEnumerable<EpicEntity>> FetchAllAsync()
        {
            const string selectSql = @"
                SELECT Id,
                       [Name],
                       CapabilityId,
                       SourceUrl,
                       CompliancyLevelId,
                       Active
                  FROM dbo.Epic;";

            return await SqlRunner.FetchAllAsync<EpicEntity>(selectSql);
        }
    }
}

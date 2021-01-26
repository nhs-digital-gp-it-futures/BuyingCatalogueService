using System;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class FrameworkCapabilitiesEntity : EntityBase
    {
        public Guid CapabilityId { get; set; }

        public string FrameworkId { get; set; }

        public bool IsFoundation { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.FrameworkCapabilities
            (
                CapabilityId,
                FrameworkId,
                IsFoundation
            )
            VALUES
            (
                @CapabilityId,
                @FrameworkId,
                @IsFoundation
            );";
    }
}

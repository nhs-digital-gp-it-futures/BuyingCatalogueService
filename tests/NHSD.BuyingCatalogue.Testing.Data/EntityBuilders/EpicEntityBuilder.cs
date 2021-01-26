using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class EpicEntityBuilder
    {
        private readonly EpicEntity epicEntity;

        public EpicEntityBuilder()
        {
            epicEntity = new EpicEntity
            {
                Id = "Default Epic",
                Name = "Name",
                CapabilityId = Guid.NewGuid(),
                SourceUrl = "url",
                CompliancyLevelId = (int)CompliancyLevel.May,
                Active = false,
            };
        }

        public enum CompliancyLevel
        {
            // ReSharper disable once UnusedMember.Global (CA1008: enums should have zero value)
            Undefined = 0,

            Must = 1,
            Should = 2,
            May = 3,
        }

        public static EpicEntityBuilder Create()
        {
            return new();
        }

        public EpicEntityBuilder WithId(string id)
        {
            epicEntity.Id = id;
            return this;
        }

        public EpicEntityBuilder WithName(string name)
        {
            epicEntity.Name = name;
            return this;
        }

        public EpicEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            epicEntity.CapabilityId = capabilityId;
            return this;
        }

        public EpicEntityBuilder WithCompliancyLevel(CompliancyLevel compliancyLevel)
        {
            epicEntity.CompliancyLevelId = (int)compliancyLevel;
            return this;
        }

        public EpicEntityBuilder WithActive(bool active)
        {
            epicEntity.Active = active;
            return this;
        }

        public EpicEntity Build()
        {
            return epicEntity;
        }
    }
}

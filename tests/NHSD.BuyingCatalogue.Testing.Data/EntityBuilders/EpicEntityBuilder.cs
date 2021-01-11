using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class EpicEntityBuilder
    {
        private readonly EpicEntity _epicEntity;

        public EpicEntityBuilder()
        {
            _epicEntity = new EpicEntity
            {
                Id = "Default Epic",
                Name = "Name",
                CapabilityId = Guid.NewGuid(),
                SourceUrl = "url",
                CompliancyLevelId = (int)CompliancyLevel.May,
                Active = false
            };
        }

        public enum CompliancyLevel
        {
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
            _epicEntity.Id = id;
            return this;
        }

        public EpicEntityBuilder WithName(string name)
        {
            _epicEntity.Name = name;
            return this;
        }

        public EpicEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            _epicEntity.CapabilityId = capabilityId;
            return this;
        }

        public EpicEntityBuilder WithCompliancyLevel(CompliancyLevel compliancyLevel)
        {
            _epicEntity.CompliancyLevelId = (int)compliancyLevel;
            return this;
        }

        public EpicEntityBuilder WithActive(bool active)
        {
            _epicEntity.Active = active;
            return this;
        }

        public EpicEntity Build()
        {
            return _epicEntity;
        }
    }
}

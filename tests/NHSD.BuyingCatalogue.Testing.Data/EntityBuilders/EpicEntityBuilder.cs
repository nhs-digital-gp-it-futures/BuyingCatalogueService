using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class EpicEntityBuilder
    {
        private readonly EpicEntity _epicEntity;
        private readonly int _compliancyLevelId = 1;

        public enum CompliancyLevel
        {
            May = 1,
            Must= 2
        }

        public static EpicEntityBuilder Create()
        {
            return new EpicEntityBuilder();
        }

        public EpicEntityBuilder()
        {
            var random = new Random();

            _epicEntity = new EpicEntity()
            {
                Id = "Default Epic",
                Name = "Name",
                CapabilityId = Guid.NewGuid(),
                SourceUrl = "url",
                CompliancyLevelId = _compliancyLevelId,
                Active = false
            };
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

        public EpicEntityBuilder WithSourceUrl(string url)
        {
            _epicEntity.SourceUrl = url;
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

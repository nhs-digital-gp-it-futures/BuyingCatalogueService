using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CapabilityEntityBuilder
    {
        private readonly CapabilityEntity _capabilityEntity;

        public static CapabilityEntityBuilder Create()
        {
            return new CapabilityEntityBuilder();
        }

        public CapabilityEntityBuilder()
        {
            //Default
            _capabilityEntity = new CapabilityEntity
            {
                Id = Guid.NewGuid(),
                CapabilityRef = "Ref",
                Version = "1.0",
                StatusId = 1,
                Name = "Capability",
                Description = "Capability Description",
                EffectiveDate = DateTime.Today,
                CategoryId = 0
            };
        }

        public CapabilityEntityBuilder WithId(Guid id)
        {
            _capabilityEntity.Id = id;
            return this;
        }

        public CapabilityEntityBuilder WithCapabilityRef(string capabilityRef)
        {
            _capabilityEntity.CapabilityRef = capabilityRef;
            return this;
        }

        public CapabilityEntityBuilder WithVersion(string version)
        {
            _capabilityEntity.Version = version;
            return this;
        }

        public CapabilityEntityBuilder WithPreviousVersion(string previousVersion)
        {
            _capabilityEntity.PreviousVersion = previousVersion;
            return this;
        }

        public CapabilityEntityBuilder WithStatusId(int statusId)
        {
            _capabilityEntity.StatusId = statusId;
            return this;
        }

        public CapabilityEntityBuilder WithName(string name)
        {
            _capabilityEntity.Name = name;
            return this;
        }

        public CapabilityEntityBuilder WithDescription(string description)
        {
            _capabilityEntity.Description = description;
            return this;
        }

        public CapabilityEntityBuilder WithSourceUrl(string sourceUrl)
        {
            _capabilityEntity.SourceUrl = sourceUrl;
            return this;
        }

        public CapabilityEntityBuilder WithEffectiveDate(DateTime effectiveDate)
        {
            _capabilityEntity.EffectiveDate = effectiveDate;
            return this;
        }

        public CapabilityEntityBuilder WithCategoryId(int categoryId)
        {
            _capabilityEntity.CategoryId = categoryId;
            return this;
        }

        public CapabilityEntity Build()
        {
            return _capabilityEntity;
        }
    }
}

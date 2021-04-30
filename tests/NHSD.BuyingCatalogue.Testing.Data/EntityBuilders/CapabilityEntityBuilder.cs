using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CapabilityEntityBuilder
    {
        private readonly CapabilityEntity capabilityEntity;

        public CapabilityEntityBuilder()
        {
            capabilityEntity = new CapabilityEntity
            {
                Id = Guid.NewGuid(),
                CapabilityRef = "Ref",
                Version = "1.0",
                StatusId = 1,
                Name = "Capability",
                Description = "Capability Description",
                EffectiveDate = DateTime.Today,
                CategoryId = 0,
            };
        }

        public static CapabilityEntityBuilder Create()
        {
            return new();
        }

        public CapabilityEntityBuilder WithId(Guid id)
        {
            capabilityEntity.Id = id;
            return this;
        }

        public CapabilityEntityBuilder WithCapabilityRef(string capabilityRef)
        {
            capabilityEntity.CapabilityRef = capabilityRef;
            return this;
        }

        public CapabilityEntityBuilder WithVersion(string version)
        {
            capabilityEntity.Version = version;
            return this;
        }

        public CapabilityEntityBuilder WithName(string name)
        {
            capabilityEntity.Name = name;
            return this;
        }

        public CapabilityEntityBuilder WithDescription(string description)
        {
            capabilityEntity.Description = description;
            return this;
        }

        public CapabilityEntityBuilder WithSourceUrl(string sourceUrl)
        {
            capabilityEntity.SourceUrl = sourceUrl;
            return this;
        }

        public CapabilityEntityBuilder WithCategoryId(int categoryId)
        {
            capabilityEntity.CategoryId = categoryId;
            return this;
        }

        public CapabilityEntity Build()
        {
            return capabilityEntity;
        }
    }
}

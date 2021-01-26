using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class FrameworkCapabilitiesEntityBuilder
    {
        private readonly FrameworkCapabilitiesEntity frameworkCapabilitiesEntity;

        public FrameworkCapabilitiesEntityBuilder()
        {
            frameworkCapabilitiesEntity = new FrameworkCapabilitiesEntity
            {
                CapabilityId = Guid.NewGuid(),

                // ReSharper disable once StringLiteralTypo
                FrameworkId = "NHSDGP001",
                IsFoundation = true,
            };
        }

        public static FrameworkCapabilitiesEntityBuilder Create()
        {
            return new();
        }

        public FrameworkCapabilitiesEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            frameworkCapabilitiesEntity.CapabilityId = capabilityId;
            return this;
        }

        public FrameworkCapabilitiesEntityBuilder WithIsFoundation(bool isFoundation)
        {
            frameworkCapabilitiesEntity.IsFoundation = isFoundation;
            return this;
        }

        public FrameworkCapabilitiesEntity Build()
        {
            return frameworkCapabilitiesEntity;
        }
    }
}

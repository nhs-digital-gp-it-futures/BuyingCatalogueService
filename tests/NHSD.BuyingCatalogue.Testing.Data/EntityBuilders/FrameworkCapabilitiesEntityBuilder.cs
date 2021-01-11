using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class FrameworkCapabilitiesEntityBuilder
    {
        private readonly FrameworkCapabilitiesEntity _frameworkCapabilitiesEntity;

        public static FrameworkCapabilitiesEntityBuilder Create()
        {
            return new();
        }

        public FrameworkCapabilitiesEntityBuilder()
        {
            //Default
            _frameworkCapabilitiesEntity = new FrameworkCapabilitiesEntity
            {
                CapabilityId = Guid.NewGuid(),
                FrameworkId = "NHSDGP001",
                IsFoundation = true
            };
        }

        public FrameworkCapabilitiesEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            _frameworkCapabilitiesEntity.CapabilityId = capabilityId;
            return this;
        }

        public FrameworkCapabilitiesEntityBuilder WithFrameworkId(string frameworkId)
        {
            _frameworkCapabilitiesEntity.FrameworkId = frameworkId;
            return this;
        }

        public FrameworkCapabilitiesEntityBuilder WithIsFoundation(bool isFoundation)
        {
            _frameworkCapabilitiesEntity.IsFoundation = isFoundation;
            return this;
        }

        public FrameworkCapabilitiesEntity Build()
        {
            return _frameworkCapabilitiesEntity;
        }
    }
}

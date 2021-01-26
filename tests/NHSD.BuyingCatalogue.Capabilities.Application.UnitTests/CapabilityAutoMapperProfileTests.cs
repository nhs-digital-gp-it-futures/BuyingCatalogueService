using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.Capabilities.Application.Domain;
using NHSD.BuyingCatalogue.Capabilities.Application.Mapping;
using NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Capabilities.Application.UnitTests
{
    [TestFixture]
    internal sealed class CapabilityAutoMapperProfileTests
    {
        private CapabilityAutoMapperProfile capabilityAutoMapperProfile;

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(CapabilityDto));
        }

        [SetUp]
        public void SetUp()
        {
            capabilityAutoMapperProfile = new CapabilityAutoMapperProfile();
        }

        [Test]
        public void ProfileMatchesMapping()
        {
            var typeMapConfigurations = ((IProfileConfiguration)capabilityAutoMapperProfile).TypeMapConfigs;

            var mappings = typeMapConfigurations.Select(
                tmc => new KeyValuePair<Type, Type>(tmc.SourceType, tmc.DestinationType));

            SupportedMappings().Should().BeEquivalentTo(mappings);
        }
    }
}

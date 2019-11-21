using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.Application.Capabilities.Domain;
using NHSD.BuyingCatalogue.Application.Capabilities.Mapping;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Capabilities
{
    [TestFixture]
    public sealed class CapabilityAutoMapperProfileTests
    {
        private CapabilityAutoMapperProfile _capabilityAutoMapperProfile;

        [SetUp]
        public void SetUp()
        {
            _capabilityAutoMapperProfile = new CapabilityAutoMapperProfile();
        }

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(CapabilityDto));
        }

        [Test]
        public void Profile_MatchesMapping()
        {
            var configs = ((IProfileConfiguration)_capabilityAutoMapperProfile).TypeMapConfigs;

            var mappings = configs.Select(tmc => new KeyValuePair<Type, Type>(tmc.SourceType, tmc.DestinationType));

            SupportedMappings().Should().BeEquivalentTo(mappings);
        }
    }
}

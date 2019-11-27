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

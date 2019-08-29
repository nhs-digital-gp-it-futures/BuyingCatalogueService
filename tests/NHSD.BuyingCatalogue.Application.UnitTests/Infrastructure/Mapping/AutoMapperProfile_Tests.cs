using AutoMapper.Configuration;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain;
using NHSD.BuyingCatalogue.Domain.Entities;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Infrastructure.Mapping
{
    [TestFixture]
    public sealed class AutoMapperProfile_Tests
    {
        private AutoMapperProfile _profile;

        [SetUp]
        public void SetUp()
        {
            _profile = new AutoMapperProfile();
        }

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(SolutionSummaryViewModel));
            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(SolutionCapabilityViewModel));
            yield return new KeyValuePair<Type, Type>(typeof(Organisation), typeof(SolutionOrganisationViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(CapabilityViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(SolutionByIdViewModel));
        }

        [Test]
        public void Profile_MatchesMapping()
        {
            var configs = ((IProfileConfiguration)_profile).TypeMapConfigs;

            var mappings = configs.Select(tmc => new KeyValuePair<Type, Type>(tmc.SourceType, tmc.DestinationType));

            SupportedMappings().ShouldBe(mappings, ignoreOrder: true);
        }
    }
}

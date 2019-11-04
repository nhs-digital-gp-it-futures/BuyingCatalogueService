using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.SolutionList.Domain;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Application.Capabilities.Domain;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NUnit.Framework;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Infrastructure.Mapping
{
    [TestFixture]
    public sealed class AutoMapperProfileTests
    {
        private AutoMapperProfile _profile;

        [SetUp]
        public void SetUp()
        {
            _profile = new AutoMapperProfile();
        }

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItem), typeof(SolutionSummaryViewModel));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItemCapability), typeof(SolutionCapabilityViewModel));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItemOrganisation), typeof(SolutionOrganisationViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(CapabilityViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(UpdateSolutionSummaryViewModel), typeof(Solution));

            yield return new KeyValuePair<Type, Type>(typeof(UpdateSolutionFeaturesViewModel), typeof(Solution));
        }

        [Test]
        public void Profile_MatchesMapping()
        {
            var configs = ((IProfileConfiguration)_profile).TypeMapConfigs;

            var mappings = configs.Select(tmc => new KeyValuePair<Type, Type>(tmc.SourceType, tmc.DestinationType));

            SupportedMappings().Should().BeEquivalentTo(mappings);
        }
    }
}

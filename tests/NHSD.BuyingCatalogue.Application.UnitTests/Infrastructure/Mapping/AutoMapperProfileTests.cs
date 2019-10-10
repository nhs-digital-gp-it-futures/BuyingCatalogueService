using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;
using NHSD.BuyingCatalogue.Domain.Entities.Organisations;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;

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
            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(SolutionSummaryViewModel));
            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(SolutionCapabilityViewModel));
            yield return new KeyValuePair<Type, Type>(typeof(Organisation), typeof(SolutionOrganisationViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(Capability), typeof(CapabilityViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(SolutionByIdViewModel));

            yield return new KeyValuePair<Type, Type>(typeof(UpdateSolutionViewModel), typeof(Solution));
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

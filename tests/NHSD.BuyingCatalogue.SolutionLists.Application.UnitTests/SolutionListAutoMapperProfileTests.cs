using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.SolutionLists.Application.Domain;
using NHSD.BuyingCatalogue.SolutionLists.Application.Mapping;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests
{
    [TestFixture]
    internal sealed class SolutionListAutoMapperProfileTests
    {
        private SolutionListAutoMapperProfile solutionListAutoMapperProfile;

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(SolutionList), typeof(SolutionListDto));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionList), typeof(ISolutionList));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItem), typeof(ISolutionSummary));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItem), typeof(SolutionSummaryDto));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItemCapability), typeof(SolutionCapabilityDto));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItemSupplier), typeof(SolutionSupplierDto));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItemCapability), typeof(ISolutionCapability));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionListItemSupplier), typeof(ISolutionSupplier));
        }

        [SetUp]
        public void SetUp()
        {
            solutionListAutoMapperProfile = new SolutionListAutoMapperProfile();
        }

        [Test]
        public void ProfileMatchesMapping()
        {
            var typeMapConfigurations = ((IProfileConfiguration)solutionListAutoMapperProfile).TypeMapConfigs;

            var mappings = typeMapConfigurations.Select(
                tmc => new KeyValuePair<Type, Type>(tmc.SourceType, tmc.DestinationType));

            SupportedMappings().Should().BeEquivalentTo(mappings);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Mapping;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public sealed class SolutionAutoMapperProfileTests
    {
        private SolutionAutoMapperProfile _solutionAutoMapperProfile;

        [SetUp]
        public void SetUp()
        {
            _solutionAutoMapperProfile = new SolutionAutoMapperProfile();
        }

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(UpdateSolutionSummaryViewModel), typeof(Solution));
            yield return new KeyValuePair<Type, Type>(typeof(UpdateSolutionFeaturesViewModel), typeof(Solution));
            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(SolutionDto));
            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(ISolution));
            yield return new KeyValuePair<Type, Type>(typeof(ClientApplication), typeof(ClientApplicationDto));
            yield return new KeyValuePair<Type, Type>(typeof(ClientApplication), typeof(IClientApplication));
            yield return new KeyValuePair<Type, Type>(typeof(Plugins), typeof(PluginsDto));
            yield return new KeyValuePair<Type, Type>(typeof(Plugins), typeof(IPlugins));
            yield return new KeyValuePair<Type, Type>(typeof(Contact), typeof(ContactDto));
            yield return new KeyValuePair<Type, Type>(typeof(Contact), typeof(IContact));
            yield return new KeyValuePair<Type, Type>(typeof(MobileOperatingSystems), typeof(MobileOperatingSystemsDto));
            yield return new KeyValuePair<Type, Type>(typeof(MobileOperatingSystems), typeof(IMobileOperatingSystems));
            yield return new KeyValuePair<Type, Type>(typeof(MobileConnectionDetails), typeof(MobileConnectionDetailsDto));
            yield return new KeyValuePair<Type, Type>(typeof(MobileConnectionDetails), typeof(IMobileConnectionDetails));
            yield return new KeyValuePair<Type, Type>(typeof(MobileMemoryAndStorage), typeof(MobileMemoryAndStorageDto));
            yield return new KeyValuePair<Type, Type>(typeof(MobileMemoryAndStorage), typeof(IMobileMemoryAndStorage));
            yield return new KeyValuePair<Type, Type>(typeof(MobileThirdParty), typeof(MobileThirdPartyDto));
            yield return new KeyValuePair<Type, Type>(typeof(MobileThirdParty), typeof(IMobileThirdParty));
            yield return new KeyValuePair<Type, Type>(typeof(NativeDesktopThirdParty), typeof(NativeDesktopThirdPartyDto));
            yield return new KeyValuePair<Type, Type>(typeof(NativeDesktopThirdParty), typeof(INativeDesktopThirdParty));
        }

        [Test]
        public void ProfileMatchesMapping()
        {
            var configs = ((IProfileConfiguration)_solutionAutoMapperProfile).TypeMapConfigs;

            var mappings = configs.Select(tmc => new KeyValuePair<Type, Type>(tmc.SourceType, tmc.DestinationType));

            SupportedMappings().Should().BeEquivalentTo(mappings);
        }
    }
}

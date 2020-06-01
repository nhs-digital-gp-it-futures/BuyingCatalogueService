using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Hostings;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Mapping;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetImplementationTimescalesBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSuppliersByName;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public sealed class SolutionAutoMapperProfileTests
    {
        private SolutionAutoMapperProfile _solutionAutoMapperProfile;

        public static IEnumerable<KeyValuePair<Type, Type>> SupportedMappings()
        {
            yield return new KeyValuePair<Type, Type>(typeof(IUpdateSolutionSummary), typeof(Solution));
            yield return new KeyValuePair<Type, Type>(typeof(IUpdateSolutionFeatures), typeof(Solution));
            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(SolutionDto));
            yield return new KeyValuePair<Type, Type>(typeof(Solution), typeof(ISolution));
            yield return new KeyValuePair<Type, Type>(typeof(ClientApplication), typeof(ClientApplicationDto));
            yield return new KeyValuePair<Type, Type>(typeof(ClientApplication), typeof(IClientApplication));
            yield return new KeyValuePair<Type, Type>(typeof(Hosting), typeof(HostingDto));
            yield return new KeyValuePair<Type, Type>(typeof(Hosting), typeof(IHosting));
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
            yield return new KeyValuePair<Type, Type>(typeof(NativeDesktopMemoryAndStorage), typeof(NativeDesktopMemoryAndStorageDto));
            yield return new KeyValuePair<Type, Type>(typeof(NativeDesktopMemoryAndStorage), typeof(INativeDesktopMemoryAndStorage));
            yield return new KeyValuePair<Type, Type>(typeof(PublicCloud), typeof(PublicCloudDto));
            yield return new KeyValuePair<Type, Type>(typeof(PublicCloud), typeof(IPublicCloud));
            yield return new KeyValuePair<Type, Type>(typeof(PrivateCloud), typeof(PrivateCloudDto));
            yield return new KeyValuePair<Type, Type>(typeof(PrivateCloud), typeof(IPrivateCloud));
            yield return new KeyValuePair<Type, Type>(typeof(HybridHostingType), typeof(HybridHostingTypeDto));
            yield return new KeyValuePair<Type, Type>(typeof(HybridHostingType), typeof(IHybridHostingType));
            yield return new KeyValuePair<Type, Type>(typeof(OnPremise), typeof(OnPremiseDto));
            yield return new KeyValuePair<Type, Type>(typeof(OnPremise), typeof(IOnPremise));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionSupplier), typeof(SolutionSupplierDto));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionSupplier), typeof(ISolutionSupplier));
            yield return new KeyValuePair<Type, Type>(typeof(Supplier), typeof(SupplierDto));
            yield return new KeyValuePair<Type, Type>(typeof(Supplier), typeof(ISupplier));
            yield return new KeyValuePair<Type, Type>(typeof(SupplierAddress), typeof(ISupplierAddress));
            yield return new KeyValuePair<Type, Type>(typeof(RoadMap), typeof(RoadMapDto));
            yield return new KeyValuePair<Type, Type>(typeof(RoadMap), typeof(IRoadMap));
            yield return new KeyValuePair<Type, Type>(typeof(Integrations), typeof(IntegrationsDto));
            yield return new KeyValuePair<Type, Type>(typeof(Integrations), typeof(IIntegrations));
            yield return new KeyValuePair<Type, Type>(typeof(ImplementationTimescales), typeof(ImplementationTimescalesDto));
            yield return new KeyValuePair<Type, Type>(typeof(ImplementationTimescales), typeof(IImplementationTimescales));
            yield return new KeyValuePair<Type, Type>(typeof(ClaimedCapability), typeof(ClaimedCapabilityDto));
            yield return new KeyValuePair<Type, Type>(typeof(ClaimedCapability), typeof(IClaimedCapability));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionDocument), typeof(SolutionDocumentDto));
            yield return new KeyValuePair<Type, Type>(typeof(SolutionDocument), typeof(ISolutionDocument));
            yield return new KeyValuePair<Type, Type>(typeof(ClaimedCapabilityEpic), typeof(ClaimedCapabilityEpicDto));
            yield return new KeyValuePair<Type, Type>(typeof(ClaimedCapabilityEpic), typeof(IClaimedCapabilityEpic));
        }

        [SetUp]
        public void SetUp()
        {
            _solutionAutoMapperProfile = new SolutionAutoMapperProfile();
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

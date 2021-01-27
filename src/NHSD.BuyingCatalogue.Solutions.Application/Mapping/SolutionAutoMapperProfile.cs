﻿using AutoMapper;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.HostingTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetAdditionalServiceByAdditionalServiceId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetImplementationTimescalesBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSuppliersByName;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Mapping
{
    public sealed class SolutionAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionAutoMapperProfile" /> class.
        /// </summary>
        public SolutionAutoMapperProfile()
        {
            CreateMap<IUpdateSolutionSummary, Solution>()
                .ForMember(destination => destination.AboutUrl, options => options.MapFrom(source => source.Link));
            CreateMap<IUpdateSolutionFeatures, Solution>()
                .ForMember(destination => destination.Features, options => options.MapFrom(source => source.Listing));

            CreateMap<Solution, SolutionDto>();
            CreateMap<Solution, ISolution>().As<SolutionDto>();
            CreateMap<ClientApplication, ClientApplicationDto>();
            CreateMap<ClientApplication, IClientApplication>().As<ClientApplicationDto>();
            CreateMap<Hosting, HostingDto>();
            CreateMap<Hosting, IHosting>().As<HostingDto>();
            CreateMap<Plugins, PluginsDto>();
            CreateMap<Plugins, IPlugins>().As<PluginsDto>();
            CreateMap<Contact, ContactDto>();
            CreateMap<Contact, IContact>().As<ContactDto>();
            CreateMap<MobileOperatingSystems, MobileOperatingSystemsDto>();
            CreateMap<MobileOperatingSystems, IMobileOperatingSystems>().As<MobileOperatingSystemsDto>();
            CreateMap<MobileConnectionDetails, MobileConnectionDetailsDto>();
            CreateMap<MobileConnectionDetails, IMobileConnectionDetails>().As<MobileConnectionDetailsDto>();
            CreateMap<MobileMemoryAndStorage, MobileMemoryAndStorageDto>();
            CreateMap<MobileMemoryAndStorage, IMobileMemoryAndStorage>().As<MobileMemoryAndStorageDto>();
            CreateMap<MobileThirdParty, MobileThirdPartyDto>();
            CreateMap<MobileThirdParty, IMobileThirdParty>().As<MobileThirdPartyDto>();
            CreateMap<NativeDesktopThirdParty, NativeDesktopThirdPartyDto>();
            CreateMap<NativeDesktopThirdParty, INativeDesktopThirdParty>().As<NativeDesktopThirdPartyDto>();
            CreateMap<NativeDesktopMemoryAndStorage, NativeDesktopMemoryAndStorageDto>();
            CreateMap<NativeDesktopMemoryAndStorage, INativeDesktopMemoryAndStorage>()
                .As<NativeDesktopMemoryAndStorageDto>();
            CreateMap<PublicCloud, PublicCloudDto>();
            CreateMap<PublicCloud, IPublicCloud>().As<PublicCloudDto>();
            CreateMap<PrivateCloud, PrivateCloudDto>();
            CreateMap<PrivateCloud, IPrivateCloud>().As<PrivateCloudDto>();
            CreateMap<HybridHostingType, HybridHostingTypeDto>();
            CreateMap<HybridHostingType, IHybridHostingType>().As<HybridHostingTypeDto>();
            CreateMap<OnPremise, OnPremiseDto>();
            CreateMap<OnPremise, IOnPremise>().As<OnPremiseDto>();
            CreateMap<SolutionSupplier, SolutionSupplierDto>();
            CreateMap<SolutionSupplier, ISolutionSupplier>().As<SolutionSupplierDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Supplier, ISupplier>().As<SupplierDto>();
            CreateMap<SupplierAddress, ISupplierAddress>().As<SupplierAddressDto>();
            CreateMap<RoadMap, RoadMapDto>();
            CreateMap<RoadMap, IRoadMap>().As<RoadMapDto>();
            CreateMap<Integrations, IntegrationsDto>();
            CreateMap<Integrations, IIntegrations>().As<IntegrationsDto>();
            CreateMap<ImplementationTimescales, ImplementationTimescalesDto>();
            CreateMap<ImplementationTimescales, IImplementationTimescales>().As<ImplementationTimescalesDto>();
            CreateMap<ClaimedCapability, ClaimedCapabilityDto>();
            CreateMap<ClaimedCapability, IClaimedCapability>().As<ClaimedCapabilityDto>();
            CreateMap<SolutionDocument, SolutionDocumentDto>();
            CreateMap<SolutionDocument, ISolutionDocument>().As<SolutionDocumentDto>();
            CreateMap<ClaimedCapabilityEpic, ClaimedCapabilityEpicDto>();
            CreateMap<ClaimedCapabilityEpic, IClaimedCapabilityEpic>().As<ClaimedCapabilityEpicDto>();
            CreateMap<CataloguePriceBase, CataloguePriceDto>();
            CreateMap<CataloguePriceBase, ICataloguePrice>().As<CataloguePriceDto>();
            CreateMap<PricingUnit, PricingUnitDto>();
            CreateMap<PricingUnit, IPricingUnit>().As<PricingUnitDto>();
            CreateMap<TimeUnit, TimeUnitDto>();
            CreateMap<TimeUnit, ITimeUnit>().As<TimeUnitDto>();
            CreateMap<TieredPrice, TieredPriceDto>();
            CreateMap<TieredPrice, ITieredPrice>().As<TieredPriceDto>();
            CreateMap<AdditionalService, AdditionalServiceDto>();
            CreateMap<AdditionalService, IAdditionalService>().As<AdditionalServiceDto>();
        }
    }
}

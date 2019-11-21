using AutoMapper;
using NHSD.BuyingCatalogue.Application.Capabilities.Domain;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Mapping
{
    public sealed class CapabilityAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CapabilityAutoMapperProfile"/> class.
        /// </summary>
        public CapabilityAutoMapperProfile()
        {
            CreateMap<Capability, CapabilityDto>();
        }
    }
}

using AutoMapper;
using NHSD.BuyingCatalogue.Capabilities.Application.Domain;
using NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Mapping
{
    public sealed class CapabilityAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilityAutoMapperProfile"/> class.
        /// </summary>
        public CapabilityAutoMapperProfile()
        {
            CreateMap<Capability, CapabilityDto>();
        }
    }
}

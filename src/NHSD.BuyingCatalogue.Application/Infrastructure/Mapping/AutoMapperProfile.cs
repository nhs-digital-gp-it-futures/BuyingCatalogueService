using AutoMapper;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Mapping
{
    public sealed class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AutoMapperProfile"/> class.
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Solution, SolutionSummaryViewModel>();
            CreateMap<Capability, SolutionCapabilityViewModel>();
            CreateMap<Organisation, SolutionOrganisationViewModel>();

            CreateMap<Capability, CapabilityViewModel>();

            CreateMap<Solution, SolutionByIdViewModel>();
        }
    }
}

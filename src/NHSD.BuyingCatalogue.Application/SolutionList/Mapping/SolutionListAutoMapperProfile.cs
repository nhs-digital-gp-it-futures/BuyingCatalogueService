using AutoMapper;
using NHSD.BuyingCatalogue.Application.SolutionList.Domain;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Contracts.SolutionList;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Mapping
{
    /// <summary>
    /// A profile for AutoMapper to define the mapping between entities and view models.
    /// </summary>
    public sealed class SolutionListAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionListAutoMapperProfile"/> class.
        /// </summary>
        public SolutionListAutoMapperProfile()
        {
            CreateMap<SolutionList.Domain.SolutionList, SolutionListDto>();
            CreateMap<SolutionList.Domain.SolutionList, ISolutionList>().As<SolutionListDto>();
            CreateMap<SolutionListItem, SolutionSummaryDto>();
            CreateMap<SolutionListItem, ISolutionSummary>().As<SolutionSummaryDto>();
            CreateMap<SolutionListItemCapability, SolutionCapabilityDto>();
            CreateMap<SolutionListItemCapability, ISolutionCapability>().As<SolutionCapabilityDto>();
            CreateMap<SolutionListItemOrganisation, SolutionOrganisationDto>();
            CreateMap<SolutionListItemOrganisation, ISolutionOrganisation>().As<SolutionOrganisationDto>();
        }
    }
}

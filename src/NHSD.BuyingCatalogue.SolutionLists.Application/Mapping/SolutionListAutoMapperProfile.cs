using AutoMapper;
using NHSD.BuyingCatalogue.Contracts.SolutionList;
using NHSD.BuyingCatalogue.SolutionLists.Application.Domain;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Mapping
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
            CreateMap<SolutionList, SolutionListDto>();
            CreateMap<SolutionList, ISolutionList>().As<SolutionListDto>();
            CreateMap<SolutionListItem, SolutionSummaryDto>();
            CreateMap<SolutionListItem, ISolutionSummary>().As<SolutionSummaryDto>();
            CreateMap<SolutionListItemCapability, SolutionCapabilityDto>();
            CreateMap<SolutionListItemCapability, ISolutionCapability>().As<SolutionCapabilityDto>();
            CreateMap<SolutionListItemOrganisation, SolutionOrganisationDto>();
            CreateMap<SolutionListItemOrganisation, ISolutionOrganisation>().As<SolutionOrganisationDto>();
        }
    }
}

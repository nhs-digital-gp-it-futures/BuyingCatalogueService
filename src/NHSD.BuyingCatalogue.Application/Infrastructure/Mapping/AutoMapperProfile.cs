using AutoMapper;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;
using NHSD.BuyingCatalogue.Domain.Entities.Organisations;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Mapping
{
    /// <summary>
    /// A profile for AutoMapper to define the mapping between entities and view models.
    /// </summary>
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

            CreateMap<UpdateSolutionSummaryViewModel, Solution>()
                .ForMember(destination => destination.AboutUrl, options => options.MapFrom(source => source.Link));

            CreateMap<UpdateSolutionFeaturesViewModel, Solution>()
                .ForMember(destination => destination.Features, options => options.MapFrom(source => JsonConvert.SerializeObject(source.Listing).ToString()));
        }
    }
}

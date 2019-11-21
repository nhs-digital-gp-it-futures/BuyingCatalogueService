using AutoMapper;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Mapping
{
    public sealed class SolutionAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionAutoMapperProfile"/> class.
        /// </summary>
        public SolutionAutoMapperProfile()
        {
            CreateMap<UpdateSolutionSummaryViewModel, Solution>()
                .ForMember(destination => destination.AboutUrl, options => options.MapFrom(source => source.Link));
            CreateMap<UpdateSolutionFeaturesViewModel, Solution>()
                .ForMember(destination => destination.Features, options => options.MapFrom(source => source.Listing));

            CreateMap<Solution, SolutionDto>();
            CreateMap<Solution, ISolution>().As<SolutionDto>();
            CreateMap<ClientApplication, ClientApplicationDto>();
            CreateMap<ClientApplication, IClientApplication>().As<ClientApplicationDto>();
            CreateMap<Plugins, PluginsDto>();
            CreateMap<Plugins, IPlugins>().As<PluginsDto>();
            CreateMap<Contact, ContactDto>();
            CreateMap<Contact, IContact>().As<ContactDto>();
        }
    }
}

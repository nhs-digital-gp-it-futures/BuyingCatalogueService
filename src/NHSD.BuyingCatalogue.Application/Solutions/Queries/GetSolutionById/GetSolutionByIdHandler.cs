using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetSolutionByIdQuery"/>.
    /// </summary>
    internal sealed class GetSolutionByIdHandler : IRequestHandler<GetSolutionByIdQuery, ISolution>
    {
        private readonly SolutionReader _solutionReader;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSolutionByIdHandler"/> class.
        /// </summary>
        public GetSolutionByIdHandler(SolutionReader solutionReader, IMapper mapper)
        {
            _solutionReader = solutionReader;
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this query.</returns>
        public async Task<ISolution> Handle(GetSolutionByIdQuery request, CancellationToken cancellationToken)
            => Map(await _solutionReader.ByIdAsync(request.Id, cancellationToken));

        private ISolution Map(Solution solution)
        {
            return new SolutionDto
            {
                Id = solution.Id,
                Name = solution.Name,
                OrganisationName = solution.OrganisationName,
                Description = solution.Description,
                Summary = solution.Summary,
                AboutUrl = solution.AboutUrl,
                Features = solution.Features,
                SupplierStatus = solution.SupplierStatus,
                ClientApplication = Map(solution.ClientApplication)
            };
        }

        private IClientApplication Map(ClientApplication clientApplication)
        {
            return new ClientApplicationDto
            {
                ClientApplicationTypes = clientApplication.ClientApplicationTypes,
                BrowsersSupported = clientApplication.BrowsersSupported,
                MobileResponsive = clientApplication.MobileResponsive,
                Plugins = Map(clientApplication.Plugins)
            };
        }

        private IPlugins Map(Plugins plugins)
        {
            return plugins != null ? new PluginsDto { Required = plugins.Required, AdditionalInformation = plugins.AdditionalInformation } : null;
        }
    }
}

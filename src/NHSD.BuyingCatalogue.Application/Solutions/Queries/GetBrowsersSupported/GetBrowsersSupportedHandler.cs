using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetBrowsersSupported
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetBrowsersSupportedQuery"/>.
    /// </summary>
    internal sealed class GetBrowsersSupportedHandler : IRequestHandler<GetBrowsersSupportedQuery, GetBrowsersSupportedResult>
    {
        private readonly SolutionReader _solutionReader;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSolutionByIdHandler"/> class.
        /// </summary>
        public GetBrowsersSupportedHandler(SolutionReader solutionReader, IMapper mapper)
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
        public async Task<GetBrowsersSupportedResult> Handle(GetBrowsersSupportedQuery request, CancellationToken cancellationToken) =>
            new GetBrowsersSupportedResult((await _solutionReader.ByIdAsync(request.Id, cancellationToken))?.ClientApplication);
    }
}

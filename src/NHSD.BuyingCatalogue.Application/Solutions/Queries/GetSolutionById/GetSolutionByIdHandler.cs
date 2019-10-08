using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetSolutionByIdQuery"/>.
    /// </summary>
    internal sealed class GetSolutionByIdHandler : IRequestHandler<GetSolutionByIdQuery, GetSolutionByIdResult>
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
        public async Task<GetSolutionByIdResult> Handle(GetSolutionByIdQuery request, CancellationToken cancellationToken)
        {
            Solution solution = await _solutionReader.ByIdAsync(request.Id, cancellationToken);

            return new GetSolutionByIdResult
            {
                Solution = _mapper.Map<SolutionByIdViewModel>(solution)
            };
        }
    }
}

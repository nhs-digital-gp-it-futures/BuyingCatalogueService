using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetSolutionByIdQuery"/>.
    /// </summary>
    internal sealed class GetSolutionByIdHandler : IRequestHandler<GetSolutionByIdQuery, ISolution>
    {
        private readonly SolutionReader solutionReader;
        private readonly IMapper mapper;

        public GetSolutionByIdHandler(SolutionReader solutionReader, IMapper mapper)
        {
            this.solutionReader = solutionReader;
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this query.</returns>
        public async Task<ISolution> Handle(GetSolutionByIdQuery request, CancellationToken cancellationToken) =>
            mapper.Map<ISolution>(await solutionReader.ByIdAsync(request.Id, cancellationToken));
    }
}

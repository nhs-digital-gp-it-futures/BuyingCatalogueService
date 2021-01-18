using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.SolutionLists.Application.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Defines the request handler for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    internal sealed class ListSolutionsHandler : IRequestHandler<ListSolutionsQuery, ISolutionList>
    {
        private readonly SolutionListReader solutionListReader;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSolutionsHandler"/> class.
        /// </summary>
        /// <param name="solutionListReader">A <see cref="solutionListReader"/> instance.</param>
        /// <param name="mapper">An <see cref="IMapper"/> instance.</param>
        public ListSolutionsHandler(SolutionListReader solutionListReader, IMapper mapper)
        {
            this.solutionListReader = solutionListReader;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The result of the query.</returns>
        public async Task<ISolutionList> Handle(ListSolutionsQuery request, CancellationToken cancellationToken)
        {
            var solutionList = await solutionListReader.ListAsync(
                request.Data.CapabilityReferences,
                request.Data.IsFoundation,
                request.Data.SupplierId,
                cancellationToken);

            return mapper.Map<SolutionListDto>(solutionList);
        }
    }
}

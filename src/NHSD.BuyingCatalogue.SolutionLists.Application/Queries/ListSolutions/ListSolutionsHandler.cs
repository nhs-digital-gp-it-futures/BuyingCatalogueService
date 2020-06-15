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
        private readonly SolutionListReader _solutionListReader;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsHandler"/> class.
        /// </summary>
        public ListSolutionsHandler(SolutionListReader solutionListReader, IMapper mapper)
        {
            _solutionListReader = solutionListReader;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The result of the query.</returns>
        public async Task<ISolutionList> Handle(ListSolutionsQuery request, CancellationToken cancellationToken)
        {
            var solutionList = await _solutionListReader.ListAsync(
                request.Data.CapabilityReferences,
                request.Data.IsFoundation,
                request.Data.SupplierId,
                cancellationToken);

            return _mapper.Map<SolutionListDto>(solutionList);
        }
    }
}

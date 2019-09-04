using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions
{
    /// <summary>
    /// Defines the request handler for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    public sealed class ListSolutionsHandler : IRequestHandler<ListSolutionsQuery, ListSolutionsResult>
    {
        /// <summary>
        /// Access the persistence layer for the <see cref="Solution"/> entity.
        /// </summary>
        private ISolutionRepository SolutionsRepository { get; }

        /// <summary>
        /// The mapper to convert one object into another type.
        /// </summary>
        private IMapper Mapper { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsHandler"/> class.
        /// </summary>
        public ListSolutionsHandler(ISolutionRepository solutionsRepository, IMapper mapper)
        {
            SolutionsRepository = solutionsRepository ?? throw new ArgumentNullException(nameof(solutionsRepository));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The result of the query.</returns>
        public async Task<ListSolutionsResult> Handle(ListSolutionsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Solution> solutionList = await SolutionsRepository.ListAsync(request.CapabilityIdList, cancellationToken).ConfigureAwait(false);

            return new ListSolutionsResult
            {
                Solutions = Mapper.Map<IEnumerable<SolutionSummaryViewModel>>(solutionList)
            };
        }
    }
}

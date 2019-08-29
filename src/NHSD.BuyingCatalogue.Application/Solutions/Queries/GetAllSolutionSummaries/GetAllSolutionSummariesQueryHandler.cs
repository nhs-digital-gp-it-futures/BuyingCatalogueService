using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetAllSolutionSummariesQuery"/>.
    /// </summary>
    public sealed class GetAllSolutionSummariesQueryHandler : IRequestHandler<GetAllSolutionSummariesQuery, GetAllSolutionSummariesQueryResult>
    {
        /// <summary>
        /// Access the persistence layer for the <see cref="Solution"/> entity.
        /// </summary>
        public ISolutionRepository SolutionsRepository { get; }

        /// <summary>
        /// The mapper to convert one object into another type.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQueryHandler"/> class.
        /// </summary>
        public GetAllSolutionSummariesQueryHandler(ISolutionRepository solutionsRepository, IMapper mapper)
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
        public async Task<GetAllSolutionSummariesQueryResult> Handle(GetAllSolutionSummariesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Solution> solutionList = await SolutionsRepository.ListSolutionSummaryAsync(request.CapabilityIdList, cancellationToken).ConfigureAwait(false);

            return new GetAllSolutionSummariesQueryResult
            {
                Solutions = Mapper.Map<IEnumerable<SolutionSummaryViewModel>>(solutionList)
            };
        }
    }
}

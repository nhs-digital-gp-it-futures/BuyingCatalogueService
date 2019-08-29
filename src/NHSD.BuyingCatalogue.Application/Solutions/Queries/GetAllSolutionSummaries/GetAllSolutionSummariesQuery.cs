using System.Collections.Generic;
using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    /// <summary>
    /// Represents the query paramters for the get all solutions request.
    /// </summary>
    public sealed class GetAllSolutionSummariesQuery : IRequest<GetAllSolutionSummariesQueryResult>
    {
        private GetAllSolutionSummariesQueryFilter Filter { get; }

        /// <summary>
        /// A list of capability Ids with no duplicates.
        /// </summary>
        public ISet<string> CapabilityIdList
        {
            get
            {
                return Filter.Capabilities ?? new HashSet<string>();
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQuery"/> class.
        /// </summary>
        public GetAllSolutionSummariesQuery() : this(new GetAllSolutionSummariesQueryFilter())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQuery"/> class.
        /// </summary>
        /// <param name="capabilityIdList">List of capability identifiers to filter on.</param>
        public GetAllSolutionSummariesQuery(GetAllSolutionSummariesQueryFilter filter)
        {
            Filter = filter ?? throw new System.ArgumentNullException(nameof(filter));
        }
    }
}

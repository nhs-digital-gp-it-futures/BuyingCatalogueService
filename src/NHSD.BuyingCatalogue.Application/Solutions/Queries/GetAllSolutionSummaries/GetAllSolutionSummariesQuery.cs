using System.Collections.Generic;
using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    /// <summary>
    /// Represents the query paramters for the get all solutions request.
    /// </summary>
    public sealed class GetAllSolutionSummariesQuery : IRequest<GetAllSolutionSummariesResult>
    {
        /// <summary>
        /// Gets the filter criteria for this query.
        /// </summary>
        private GetAllSolutionSummariesFilter Filter { get; }

        /// <summary>
        /// A list of capability Ids with no duplicates.
        /// </summary>
        public ISet<string> CapabilityIdList
        {
            get
            {
                return Filter.Capabilities;
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQuery"/> class.
        /// </summary>
        public GetAllSolutionSummariesQuery() : this(new GetAllSolutionSummariesFilter())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQuery"/> class.
        /// </summary>
        /// <param name="capabilityIdList">List of capability identifiers to filter on.</param>
        public GetAllSolutionSummariesQuery(GetAllSolutionSummariesFilter filter)
        {
            Filter = filter ?? throw new System.ArgumentNullException(nameof(filter));
        }
    }
}

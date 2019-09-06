using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions
{
    /// <summary>
    /// Represents the query paramters for the get all solutions request.
    /// </summary>
    public sealed class ListSolutionsQuery : IRequest<ListSolutionsResult>
    {
        public IHttpContextAccessor Context { get; }

        /// <summary>
        /// Gets the filter criteria for this query.
        /// </summary>
        private ListSolutionsFilter Filter { get; } = new ListSolutionsFilter();

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
        /// Initialises a new instance of the <see cref="ListSolutionsQuery"/> class.
        /// </summary>
        public ListSolutionsQuery(IHttpContextAccessor context)
        {
            Context = context;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsQuery"/> class.
        /// </summary>
        /// <param name="capabilityIdList">List of capability identifiers to filter on.</param>
        public ListSolutionsQuery(IHttpContextAccessor context, ListSolutionsFilter filter) :
            this(context)
        {
            Filter = filter ?? throw new System.ArgumentNullException(nameof(filter));
        }
    }
}

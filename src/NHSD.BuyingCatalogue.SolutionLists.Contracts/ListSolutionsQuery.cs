using System;
using System.Collections.Generic;
using MediatR;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    /// <summary>
    /// Represents the query parameters for the get all solutions request.
    /// </summary>
    public sealed class ListSolutionsQuery : IRequest<ISolutionList>
    {
        /// <summary>
        /// A list of capability Ids with no duplicates.
        /// </summary>
        public ISet<Guid> CapabilityIdList;

        /// <summary>
        /// a filter criteria for solutions.
        /// </summary>
        public bool IsFoundation;

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsQuery"/> class.
        /// </summary>
        //  <param name="capabilityIdList">List of capability identifiers to filter by.</param>
        public ListSolutionsQuery(ISet<Guid> capabilityIdList, bool isFoundation)
        {
            CapabilityIdList = capabilityIdList ?? throw new ArgumentNullException();
            IsFoundation = isFoundation;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsQuery"/> class.
        /// </summary>
        public ListSolutionsQuery(bool isFoundation)
        {
            IsFoundation = isFoundation;
            CapabilityIdList = new HashSet<Guid>();
        }

        public ListSolutionsQuery()
        {
            IsFoundation = false;
            CapabilityIdList = new HashSet<Guid>();
        }
    }
}

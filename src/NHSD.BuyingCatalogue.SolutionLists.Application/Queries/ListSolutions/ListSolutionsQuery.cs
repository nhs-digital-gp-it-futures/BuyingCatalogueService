using System;
using MediatR;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Represents the query parameters for the get all solutions request.
    /// </summary>
    public sealed class ListSolutionsQuery : IRequest<ISolutionList>
    {
        public IListSolutionsQueryData Data { get; }

        public ListSolutionsQuery(IListSolutionsQueryData data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

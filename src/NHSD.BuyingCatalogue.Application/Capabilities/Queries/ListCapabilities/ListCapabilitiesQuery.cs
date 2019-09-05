using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
    /// <summary>
    /// Represents the query to retrieve a list of capabilities.
    /// </summary>
    public sealed class ListCapabilitiesQuery : IRequest<ListCapabilitiesResult>
    {
        public IHttpContextAccessor Context { get; }

        public ListCapabilitiesQuery(IHttpContextAccessor context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}

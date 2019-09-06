using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
    /// <summary>
    /// Represents the query to retrieve a list of capabilities.
    /// </summary>
    public sealed class ListCapabilitiesQuery : IRequest<ListCapabilitiesResult>
    {
        public IIdentityProvider IdProvider { get; }

        public ListCapabilitiesQuery(IIdentityProvider idProvider)
        {
            IdProvider = idProvider ?? throw new ArgumentNullException(nameof(idProvider));
        }
    }
}

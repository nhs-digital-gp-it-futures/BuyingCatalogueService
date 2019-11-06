using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
    /// <summary>
    /// Represents the query to retrieve a list of capabilities.
    /// </summary>
    public sealed class ListCapabilitiesQuery : IRequest<IEnumerable<ICapability>>
    {

    }
}

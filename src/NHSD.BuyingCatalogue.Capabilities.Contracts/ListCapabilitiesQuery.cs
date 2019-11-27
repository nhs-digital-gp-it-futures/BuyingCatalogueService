using System.Collections.Generic;
using MediatR;

namespace NHSD.BuyingCatalogue.Capabilities.Contracts
{
    /// <summary>
    /// Represents the query to retrieve a list of capabilities.
    /// </summary>
    public sealed class ListCapabilitiesQuery : IRequest<IEnumerable<ICapability>>
    {
    }
}

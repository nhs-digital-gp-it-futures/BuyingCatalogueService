using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Clients
{
    /// <summary>
    /// Defines the interface for the automatically generated <see cref="DocumentsApiClient"/>.
    /// </summary>
    public partial interface IDocumentsApiClient
    {
        public Uri BaseAddress { get; set; }
    }
}

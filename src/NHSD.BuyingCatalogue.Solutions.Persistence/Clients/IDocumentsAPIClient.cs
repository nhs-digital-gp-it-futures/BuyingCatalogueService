using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Clients
{
    /// <summary>
    /// Defines the interface for the automatically generated <see cref="DocumentsAPIClient"/>.
    /// </summary>
    public partial interface IDocumentsAPIClient
    {
        public Uri BaseAddress { get; set; }
    }
}

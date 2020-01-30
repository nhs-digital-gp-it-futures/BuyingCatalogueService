using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Clients
{
    public partial interface IDocumentsAPIClient
    {
        public Uri BaseAddress { get; set; }
    }
}

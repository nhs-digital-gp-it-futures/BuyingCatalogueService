using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Clients
{
    public partial class DocumentsAPIClient
    {
        public Uri BaseAddress { get { return _httpClient.BaseAddress; } set { _httpClient.BaseAddress = value; } }
    }
}

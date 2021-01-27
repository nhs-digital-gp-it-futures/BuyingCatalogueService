using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Clients
{
    /// <summary>
    /// Extends the automatically generated <see cref="DocumentsApiClient"/> with a base address.
    /// </summary>
    public partial class DocumentsApiClient
    {
        public Uri BaseAddress
        {
            get
            {
                return _httpClient.BaseAddress;
            }

            set
            {
                _httpClient.BaseAddress = value;
            }
        }
    }
}

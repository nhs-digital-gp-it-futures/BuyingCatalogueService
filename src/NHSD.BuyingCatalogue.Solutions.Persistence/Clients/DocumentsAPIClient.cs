using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Clients
{
    /// <summary>
    /// Extends the automatically generated <see cref="DocumentsAPIClient"/> with a base address.
    /// </summary>
    public partial class DocumentsAPIClient
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

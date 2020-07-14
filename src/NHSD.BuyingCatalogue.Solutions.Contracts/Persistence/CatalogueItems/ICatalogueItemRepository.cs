using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems
{
    public interface ICatalogueItemRepository
    {
        Task<ICatalogueItemResult> GetByIdAsync(string catalogueItemId, CancellationToken cancellationToken);

        Task<IEnumerable<ICatalogueItemResult>> ListAsync(string supplierId,
            CatalogueItemType? catalogueItemType, CancellationToken cancellationToken);
    }
}

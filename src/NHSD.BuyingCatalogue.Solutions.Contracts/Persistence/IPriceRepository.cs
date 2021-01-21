using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IPriceRepository
    {
        Task<IEnumerable<ICataloguePriceListResult>> GetPricesAsync(string catalogueItemId, CancellationToken cancellationToken);

        Task<IEnumerable<ICataloguePriceListResult>> GetPriceByPriceIdQueryAsync(int priceId, CancellationToken cancellationToken);
    }
}

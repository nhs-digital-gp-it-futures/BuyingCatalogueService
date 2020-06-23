using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IPriceRepository
    {
        Task<IEnumerable<ICataloguePriceListResult>> GetPricingBySolutionIdQueryAsync(string solutionId, CancellationToken cancellationToken);
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IPricingRepository
    {
        Task<IEnumerable<ICataloguePriceListResult>> GetPricingBySolutionIdQuery(string solutionId, CancellationToken cancellationToken);
    }
}

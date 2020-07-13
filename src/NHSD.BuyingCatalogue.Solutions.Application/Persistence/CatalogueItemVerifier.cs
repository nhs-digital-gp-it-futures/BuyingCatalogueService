using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class CatalogueItemVerifier
    {
        private readonly ICatalogueItemRepository _catalogueItemRepository;

        public CatalogueItemVerifier(ICatalogueItemRepository catalogueItemRepository)
            => _catalogueItemRepository = catalogueItemRepository;

        public async Task<bool> CheckExists(string catalogueItemId, CancellationToken cancellationToken)
            => await _catalogueItemRepository.CheckExists(catalogueItemId, cancellationToken).ConfigureAwait(false);

        public async Task ThrowWhenMissingAsync(string catalogueItemId, CancellationToken cancellationToken)
        {
            if (!await CheckExists(catalogueItemId, cancellationToken).ConfigureAwait(false))
                throw new NotFoundException("CatalogueItem", catalogueItemId);
        }
    }
}

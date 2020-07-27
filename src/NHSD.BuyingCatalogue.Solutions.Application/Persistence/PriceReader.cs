using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class PriceReader
    {
        private readonly IPriceRepository _priceRepository;

        public PriceReader(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public async Task<CataloguePriceBase> GetByPriceIdAsync(int priceId, CancellationToken cancellationToken)
        {
            var priceItems = await _priceRepository.GetPriceByPriceIdQueryAsync(priceId, cancellationToken);
            return ProcessPriceItems(priceItems).FirstOrDefault();
        }

        public async Task<IEnumerable<CataloguePriceBase>> GetPricesAsync(string catalogueItemId, CancellationToken cancellationToken)
        {
            var prices = await _priceRepository.GetPricesAsync(catalogueItemId, cancellationToken);
            return ProcessPriceItems(prices);
        }

        private static IEnumerable<CataloguePriceBase> ProcessPriceItems(IEnumerable<ICataloguePriceListResult> priceItems)
        {
            Dictionary<int, CataloguePriceBase> priceDictionary = new Dictionary<int, CataloguePriceBase>();

            foreach (var price in priceItems)
            {
                var cataloguePriceType = Enumerator.FromValue<CataloguePriceType>(price.CataloguePriceTypeId);

                if (Equals(cataloguePriceType, CataloguePriceType.Flat))
                {
                    priceDictionary.Add(price.CataloguePriceId, GetFlatPrice(price));
                }
                else if (Equals(cataloguePriceType, CataloguePriceType.Tiered))
                {
                    CataloguePriceTier tier;

                    if (priceDictionary.ContainsKey(price.CataloguePriceId))
                    {
                        tier = priceDictionary[price.CataloguePriceId] as CataloguePriceTier;

                        UpdateTierPrices(tier, price);
                    }
                    else
                    {
                        tier = GetTierPrice(price);

                        priceDictionary.Add(price.CataloguePriceId, tier);
                    }
                }
            }

            return priceDictionary.Values;
        }

        private static void UpdateTierPrices(CataloguePriceTier tier, ICataloguePriceListResult price)
        {
            tier.TieredPrices.Add(new TieredPrice(price.BandStart.GetValueOrDefault(), price.BandEnd,
                price.TieredPrice.GetValueOrDefault()));
        }

        private static CataloguePriceTier GetTierPrice(ICataloguePriceListResult price)
        {
            CataloguePriceTier tier = new CataloguePriceTier
            {
                CataloguePriceId = price.CataloguePriceId,
                CatalogueItemName = price.CatalogueItemName,
                CatalogueItemId = price.CatalogueItemId,
                CurrencyCode = price.CurrencyCode,
                PricingUnit = new PricingUnit
                {
                    Name = price.PricingUnitName,
                    Description = price.PricingUnitDescription,
                    TierName = price.PricingUnitTierName
                },
                TimeUnit = price.TimeUnitId is null ? null : Enumerator.FromValue<TimeUnit>(price.TimeUnitId.Value),
                ProvisioningType = Enumerator.FromValue<ProvisioningType>(price.ProvisioningTypeId)
            };

            UpdateTierPrices(tier, price);
            return tier;
        }

        private static CataloguePriceFlat GetFlatPrice(ICataloguePriceListResult price)
        {
            var flatPrice = new CataloguePriceFlat
            {
                CataloguePriceId = price.CataloguePriceId,
                CatalogueItemName = price.CatalogueItemName,
                CatalogueItemId = price.CatalogueItemId,
                PricingUnit = new PricingUnit
                {
                    Description = price.PricingUnitDescription,
                    Name = price.PricingUnitName,
                    TierName = price.PricingUnitTierName
                },
                TimeUnit = price.TimeUnitId is null ? null : Enumerator.FromValue<TimeUnit>(price.TimeUnitId.Value),
                CurrencyCode = price.CurrencyCode,
                Price = price.FlatPrice.GetValueOrDefault(),
                ProvisioningType = Enumerator.FromValue<ProvisioningType>(price.ProvisioningTypeId)
            };
            return flatPrice;
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class PricingReader
    {
        private readonly IPricingRepository _pricingRepository;

        public PricingReader(IPricingRepository pricingRepository)
        {
            _pricingRepository = pricingRepository;
        }

        public async Task<IEnumerable<CataloguePriceBase>> GetBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
        {
            var a = await _pricingRepository.GetPricingBySolutionIdQuery(solutionId, cancellationToken);
            Dictionary<int, CataloguePriceBase> dictionary = new Dictionary<int, CataloguePriceBase>();

            foreach (var price in a)
            {
                var b = Enumerator.FromValue<CataloguePriceType>(price.CataloguePriceTypeId);

                if (Equals(b, CataloguePriceType.Flat))
                {
                    dictionary.Add(price.CataloguePriceId, new CataloguePriceFlat
                    {
                        CataloguePriceId = price.CataloguePriceId,
                        CatalogueItemId = price.CatalogueItemId,
                        Type = CataloguePriceType.Flat.Name,
                        ProvisioningType =
                            Enumerator.FromValue<ProvisioningType>(price.ProvisioningTypeId),
                        PricingUnit = new PricingUnit
                        {
                            Description = price.PricingUnitDescription,
                            Name = price.PricingUnitName,
                            TierName = price.PricingUnitTierName
                        },
                        TimeUnit = price.TimeUnitId == 0 ? null : Enumerator.FromValue<TimeUnit>(price.TimeUnitId),
                        CurrencyCode = price.CurrencyCode,
                        Price = price.FlatPrice.GetValueOrDefault()
                    });
                }
                else if (Equals(b, CataloguePriceType.Tiered))
                {
                    CataloguePriceTier tier;

                    if (dictionary.ContainsKey(price.CataloguePriceId))
                    {
                        tier = dictionary[price.CataloguePriceId] as CataloguePriceTier;
                    }

                    else
                    {
                        tier = new CataloguePriceTier
                        {
                            CataloguePriceId = price.CataloguePriceId,
                            CatalogueItemId = price.CatalogueItemId,
                            Type = CataloguePriceType.Tiered.Name,
                            CurrencyCode = price.CurrencyCode,
                            ProvisioningType =
                                Enumerator.FromValue<ProvisioningType>(price.ProvisioningTypeId),
                            PricingUnit = new PricingUnit
                            {
                                Name = price.PricingUnitName,
                                Description = price.PricingUnitDescription,
                                TierName = price.PricingUnitTierName
                            },
                            TimeUnit = Enumerator.FromValue<TimeUnit>(price.TimeUnitId),
                            //TieringPeriod = 
                        };

                        dictionary.Add(price.CataloguePriceId, tier);
                    }

                    tier?.TieredPrices.Add(new TieredPrice(price.BandStart.GetValueOrDefault(), price.BandEnd,
                        price.TieredPrice.GetValueOrDefault()));
                }
            }

            return dictionary.Values;
        }
    }
}

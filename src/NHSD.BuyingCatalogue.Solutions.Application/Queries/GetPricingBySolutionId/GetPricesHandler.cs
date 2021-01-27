using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    internal sealed class GetPricesHandler : IRequestHandler<GetPricesQuery, IEnumerable<ICataloguePrice>>
    {
        private readonly PriceReader priceReader;
        private readonly IMapper mapper;

        public GetPricesHandler(PriceReader priceReader, IMapper mapper)
        {
            this.priceReader = priceReader;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ICataloguePrice>> Handle(GetPricesQuery request, CancellationToken cancellationToken)
        {
            var prices = await priceReader.GetPricesAsync(request.CatalogueItemId, cancellationToken);

            List<ICataloguePrice> cataloguePrices = new List<ICataloguePrice>();

            foreach (var price in prices)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (price is CataloguePriceFlat cataloguePriceFlat)
                {
                    cataloguePrices.Add(new FlatCataloguePriceDto
                    {
                        CataloguePriceId = price.CataloguePriceId,
                        CatalogueItemName = price.CatalogueItemName,
                        CatalogueItemId = price.CatalogueItemId,
                        Type = price.CataloguePriceType.Name,
                        ProvisioningType = price.ProvisioningType.Name,
                        CurrencyCode = price.CurrencyCode,
                        PricingUnit = mapper.Map<IPricingUnit>(price.PricingUnit),
                        TimeUnit = mapper.Map<ITimeUnit>(price.TimeUnit),
                        Price = cataloguePriceFlat.Price,
                    });
                }
                else if (price is CataloguePriceTier cataloguePriceTier)
                {
                    cataloguePrices.Add(new TieredCataloguePriceDto
                    {
                        CatalogueItemId = price.CatalogueItemId,
                        CatalogueItemName = price.CatalogueItemName,
                        CataloguePriceId = price.CataloguePriceId,
                        Type = price.CataloguePriceType.Name,
                        ProvisioningType = price.ProvisioningType.Name,
                        CurrencyCode = price.CurrencyCode,
                        PricingUnit = mapper.Map<IPricingUnit>(price.PricingUnit),
                        TimeUnit = mapper.Map<ITimeUnit>(price.TimeUnit),
                        TieredPrices = mapper.Map<IEnumerable<ITieredPrice>>(cataloguePriceTier.TieredPrices),
                    });
                }
            }

            return cataloguePrices;
        }
    }
}

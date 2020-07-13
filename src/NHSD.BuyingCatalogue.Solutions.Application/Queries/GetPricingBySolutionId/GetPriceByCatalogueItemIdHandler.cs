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
    internal sealed class GetPriceByCatalogueItemIdHandler : IRequestHandler<GetPriceByCatalogueItemIdQuery, IEnumerable<ICataloguePrice>>
    {
        private readonly PriceReader _priceReader;
        private readonly CatalogueItemVerifier _verifier;
        private readonly IMapper _mapper;

        public GetPriceByCatalogueItemIdHandler(PriceReader priceReader, CatalogueItemVerifier verifier, IMapper mapper)
        {
            _priceReader = priceReader;
            _verifier = verifier;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ICataloguePrice>> Handle(GetPriceByCatalogueItemIdQuery request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.CatalogueItemId, cancellationToken);
            var prices = await _priceReader.GetByCatalogueItemIdAsync(request.CatalogueItemId, cancellationToken);

            List<ICataloguePrice> cataloguePrices = new List<ICataloguePrice>();

            foreach (var price in prices)
            {
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
                        PricingUnit = _mapper.Map<IPricingUnit>(price.PricingUnit),
                        TimeUnit = _mapper.Map<ITimeUnit>(price.TimeUnit),
                        Price = cataloguePriceFlat.Price
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
                        PricingUnit = _mapper.Map<IPricingUnit>(price.PricingUnit),
                        TimeUnit = _mapper.Map<ITimeUnit>(price.TimeUnit),
                        TieredPrices = _mapper.Map<IEnumerable<ITieredPrice>>(cataloguePriceTier.TieredPrices)
                    });
                }
            }

            return cataloguePrices;
        }
    }
}

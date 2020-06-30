using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingByPriceId
{
    internal sealed class GetPricingByPriceIdHandler : IRequestHandler<GetPriceByPriceIdQuery, ICataloguePrice>
    {
        private readonly PriceReader _pricingReader;
        private readonly IMapper _mapper;

        public GetPricingByPriceIdHandler(PriceReader pricingReader, IMapper mapper)
        {
            _pricingReader = pricingReader;
            _mapper = mapper;
        }

        public async Task<ICataloguePrice> Handle(GetPriceByPriceIdQuery request, CancellationToken cancellationToken)
        {
            var price = await _pricingReader.GetByPriceIdAsync(request.PriceId, cancellationToken);

            ICataloguePrice cataloguePrice = null;

            if (price is CataloguePriceFlat cataloguePriceFlat)
            {
                cataloguePrice = new FlatCataloguePriceDto
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
                };
            }
            else if (price is CataloguePriceTier cataloguePriceTier)
            {
                cataloguePrice = new TieredCataloguePriceDto
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
                };
            }
           
            return cataloguePrice;
        }
    }
}

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
        private readonly PriceReader pricingReader;
        private readonly IMapper mapper;

        public GetPricingByPriceIdHandler(PriceReader pricingReader, IMapper mapper)
        {
            this.pricingReader = pricingReader;
            this.mapper = mapper;
        }

        public async Task<ICataloguePrice> Handle(GetPriceByPriceIdQuery request, CancellationToken cancellationToken)
        {
            var price = await pricingReader.GetByPriceIdAsync(request.PriceId, cancellationToken);

            ICataloguePrice cataloguePrice = price switch
            {
                CataloguePriceFlat cataloguePriceFlat => new FlatCataloguePriceDto
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
                },
                CataloguePriceTier cataloguePriceTier => new TieredCataloguePriceDto
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
                },
                _ => null,
            };

            return cataloguePrice;
        }
    }
}

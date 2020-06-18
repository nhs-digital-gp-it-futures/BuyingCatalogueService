using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/pricing")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PricingController : ControllerBase
    {
        [HttpGet]
        [Route("{priceId}")]
        public ActionResult<PriceResult> GetPrice(int priceId)
        {
            return new PriceResult
            {
                Id = priceId,
                Type = "flat",
                CurrencyCode = "GBP",
                ProvisioningModel = "OnDemand",
                ItemUnit = new ItemUnitResult { Name = "Consultation", Description = "Per Consultation" },
                Price = 1.64m
            };
        }

        [HttpGet]
        [Route("/api/v1/solutions/{solutionId}/pricing")]
        public ActionResult<PricingResult> Get(string solutionId)
        {
            var result = new PricingResult
            {
                Id = $"{solutionId} ID",
                Name = "name",
                Prices = new List<PriceResult>
                {
                    new PriceResult
                    {
                        Type = "flat",
                        CurrencyCode = "GBP",
                        ItemUnit = new ItemUnitResult
                        {
                            Name = "Patient",
                            Description = "Per Patient"
                        },
                        TimeUnit = new TimeUnitResult
                        {
                            Name = "Year",
                            Description = "Per Year"
                        },
                        Price = new decimal(1.64)
                    },
                    new PriceResult
                    {
                        Type = "Tiered",
                        CurrencyCode = "GBP",
                        ItemUnit = new ItemUnitResult
                        {
                            Name = "Consultation",
                            Description = "Per Consultation",
                            TierName = "Consultations"
                        },
                        TimeUnit = new TimeUnitResult
                        {
                            Name = "Month",
                            Description = "Per Month"
                        },
                        TieringPeriod = 3,
                        Tiers = new List<TierResult>
                        {
                            new TierResult
                            {
                                Start = 1,
                                End = 5,
                                Price = 700.00
                            },
                            new TierResult
                            {
                                Start = 6,
                                End = 10,
                                Price = 600.00
                            },
                            new TierResult
                            {
                                Start = 11,
                                End = 16,
                                Price = 500.00
                            },
                            new TierResult
                            {
                                Start = 16,
                                Price = 400.00
                            }
                        }
                    }
                }
            };

            return result;
        }
    }
}

﻿using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PricingController : ControllerBase
    {
        [HttpGet]
        [Route("{solutionId}/pricing")]
        public static ActionResult<PricingResult> Get(string solutionId)
        {
            var result = new PricingResult
            {
                Id = $"{solutionId} ID",
                Name = "name",
                Prices = new List<PricesResult>
                {
                    new PricesResult
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
                    new PricesResult
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
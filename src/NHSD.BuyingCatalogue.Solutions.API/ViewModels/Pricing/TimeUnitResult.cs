using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing
{
    public sealed class TimeUnitResult
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public TimeUnitResult(ITimeUnit timeUnit)
        {
            if (timeUnit is null)
            {
                throw new ArgumentNullException(nameof(timeUnit));
            }

            Name = timeUnit.Name;
            Description = timeUnit.Description;
        }
    }
}

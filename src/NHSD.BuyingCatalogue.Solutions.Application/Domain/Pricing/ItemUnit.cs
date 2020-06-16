using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class ItemUnit
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string TierName { get; set; }

        public ItemUnit(IItemUnit itemUnit)
        {
            if (itemUnit is null)
            {
                throw new ArgumentNullException(nameof(itemUnit));
            }

            Name = itemUnit.Name;
            Description = itemUnit.Description;
            TierName = itemUnit.TierName;
        }
    }
}

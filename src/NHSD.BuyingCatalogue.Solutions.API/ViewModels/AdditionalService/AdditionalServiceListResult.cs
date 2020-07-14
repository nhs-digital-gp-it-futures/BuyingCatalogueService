using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.AdditionalService
{
    public sealed class AdditionalServiceListResult
    {
        public IEnumerable<AdditionalServiceModel> AdditionalServices { get; set; }
    }
}

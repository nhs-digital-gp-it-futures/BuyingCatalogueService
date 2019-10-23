using System.Linq;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class FeaturesSection : Section
    {
        internal FeaturesSection(Solution solution)
        {
            Data = new Features(solution.Features);
            _isComplete = Data.Listing.Any(s => !string.IsNullOrWhiteSpace(s));
        }

        public override string Id => "features";

        public Features Data { get; }
    }
}

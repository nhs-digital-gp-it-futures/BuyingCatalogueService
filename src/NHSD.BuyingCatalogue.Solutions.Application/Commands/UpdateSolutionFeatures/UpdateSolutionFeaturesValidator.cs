using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    internal sealed class UpdateSolutionFeaturesValidator
    {
        public MaxLengthResult Validate(UpdateSolutionFeaturesViewModel updateSolutionFeaturesViewModel)
        {
            var listing = updateSolutionFeaturesViewModel.Listing.ToList();
            var validationResult = new MaxLengthResult();

            for (int i = 0; i < listing.Count(); i++)
            {
                if ((listing[i]?.Length ?? 0) > 100)
                {
                    validationResult.MaxLength.Add($"listing-{i + 1}");
                }
            }

            return validationResult;
        }
    }
}

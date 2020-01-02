using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    internal sealed class UpdateSolutionFeaturesValidator : IValidator<UpdateSolutionFeaturesCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionFeaturesCommand updateSolutionFeaturesCommand)
        {
            var listing = updateSolutionFeaturesCommand.UpdateSolutionFeaturesViewModel.Listing.ToList();
            var validator = new MaxLengthValidator();

            for (int i = 0; i < listing.Count(); i++)
            {
                validator.Validate(listing[i], 100, $"listing-{i + 1}");
            }

            return validator.Result();
        }
    }
}

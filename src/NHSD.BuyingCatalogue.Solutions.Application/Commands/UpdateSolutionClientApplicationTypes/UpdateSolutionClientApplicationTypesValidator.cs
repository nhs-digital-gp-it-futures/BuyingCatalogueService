using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesValidator
    {
        public RequiredResult Validate(UpdateSolutionClientApplicationTypesViewModel updateSolutionClientApplicationTypesViewModel)
        {
            var validationResult = new RequiredResult();

            if (!updateSolutionClientApplicationTypesViewModel.FilteredClientApplicationTypes.Any())
            {
                validationResult.Required.Add("client-application-types");
            }

            return validationResult;
        }
    }
}

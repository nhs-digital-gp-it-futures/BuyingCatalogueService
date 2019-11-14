using System.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesValidator
    {
        public UpdateSolutionClientApplicationTypesValidationResult Validate(UpdateSolutionClientApplicationTypesViewModel updateSolutionClientApplicationTypesViewModel)
        {
            var validationResult = new UpdateSolutionClientApplicationTypesValidationResult();

            if (!updateSolutionClientApplicationTypesViewModel.FilteredClientApplicationTypes.Any())
            {
                validationResult.Required.Add("client-application-types");
            }

            return validationResult;
        }
    }
}

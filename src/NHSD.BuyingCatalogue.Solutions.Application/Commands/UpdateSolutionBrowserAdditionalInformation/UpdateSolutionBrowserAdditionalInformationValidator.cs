using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationValidator
    {
        public MaxLengthResult Validation(UpdateSolutionBrowserAdditionalInformationViewModel updateSolutionBrowserAdditionalInformationViewModel)
        {
            var validationResult = new MaxLengthResult();

            if (updateSolutionBrowserAdditionalInformationViewModel.AdditionalInformation?.Length > 500)
            {
                validationResult.MaxLength.Add("additional-information");
            }

            return validationResult;
        }
    }
}

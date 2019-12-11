namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationValidator
    {
        public UpdateSolutionBrowserAdditionalInformationValidationResult Validation(UpdateSolutionBrowserAdditionalInformationViewModel updateSolutionBrowserAdditionalInformationViewModel)
        {
            var validationResult = new UpdateSolutionBrowserAdditionalInformationValidationResult();

            if (updateSolutionBrowserAdditionalInformationViewModel.AdditionalInformation?.Length > 500)
            {
                validationResult.MaxLength.Add("additional-information");
            }

            return validationResult;
        }
    }
}

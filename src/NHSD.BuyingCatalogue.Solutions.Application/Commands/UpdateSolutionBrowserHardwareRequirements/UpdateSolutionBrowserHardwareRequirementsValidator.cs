namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsValidator
    {
        public UpdateSolutionBrowserHardwareRequirementsValidationResult Validation(UpdateSolutionBrowserHardwareRequirementsViewModel updateSolutionHardwareRequirementsViewModel)
        {
            var validationResult = new UpdateSolutionBrowserHardwareRequirementsValidationResult();

            if (updateSolutionHardwareRequirementsViewModel.HardwareRequirements?.Length > 500)
            {
                validationResult.MaxLength.Add("hardware-requirements-description");
            }

            return validationResult;
        }
    }
}

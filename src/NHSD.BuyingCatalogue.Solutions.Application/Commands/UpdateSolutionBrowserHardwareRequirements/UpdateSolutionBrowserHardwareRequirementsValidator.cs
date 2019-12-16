using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsValidator
    {
        public MaxLengthResult Validation(UpdateSolutionBrowserHardwareRequirementsViewModel updateSolutionHardwareRequirementsViewModel)
        {
            var validationResult = new MaxLengthResult();

            if (updateSolutionHardwareRequirementsViewModel.HardwareRequirements?.Length > 500)
            {
                validationResult.MaxLength.Add("hardware-requirements-description");
            }

            return validationResult;
        }
    }
}

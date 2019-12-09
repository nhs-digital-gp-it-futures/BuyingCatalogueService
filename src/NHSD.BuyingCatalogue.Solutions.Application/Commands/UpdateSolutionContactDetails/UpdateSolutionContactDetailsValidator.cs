namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal class UpdateSolutionContactDetailsValidator
    {
        public const int FirstNameMaxLength = 35;
        public const int LastNameMaxLength = 35;
        public const int EmailMaxLength = 255;
        public const int PhoneMaxLength = 35;
        public const int DepartmentMaxLength = 35;

        public UpdateSolutionContactDetailsValidationResult Validation(
            UpdateSolutionContactDetailsViewModel updateSolutionBrowsersSupportedViewModel)
        {
            var result = new UpdateSolutionContactDetailsValidationResult();
            ValidateContact(result, updateSolutionBrowsersSupportedViewModel.Contact1, "contact1");
            ValidateContact(result, updateSolutionBrowsersSupportedViewModel.Contact2, "contact2");
            return result;
        }

        private static void ValidateContact(UpdateSolutionContactDetailsValidationResult result, UpdateSolutionContactViewModel contact, string contactTag)
        {
            if ((contact?.FirstName?.Length ?? 0) > FirstNameMaxLength)
            {
                result.MaxLength.Add($"{contactTag}-first-name");
            }

            if ((contact?.LastName?.Length ?? 0) > LastNameMaxLength)
            {
                result.MaxLength.Add($"{contactTag}-last-name");
            }

            if ((contact?.Email?.Length ?? 0) > EmailMaxLength)
            {
                result.MaxLength.Add($"{contactTag}-email-address");
            }

            if ((contact?.PhoneNumber?.Length ?? 0) > PhoneMaxLength)
            {
                result.MaxLength.Add($"{contactTag}-phone-number");
            }

            if ((contact?.Department?.Length ?? 0) > DepartmentMaxLength)
            {
                result.MaxLength.Add($"{contactTag}-department-name");
            }
        }
    }
}

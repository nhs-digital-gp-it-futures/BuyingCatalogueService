using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal class UpdateSolutionContactDetailsValidator
    {
        public MaxLengthResult Validation(UpdateSolutionContactDetailsViewModel updateSolutionBrowsersSupportedViewModel)
        => new MaxLengthValidator()
            .ValidateContact(updateSolutionBrowsersSupportedViewModel.Contact1, "contact1")
            .ValidateContact(updateSolutionBrowsersSupportedViewModel.Contact2, "contact2")
            .Result();
    }

    internal static class MaxLengthValidatorContactExtensions
    {
        private const int FirstNameMaxLength = 35;
        private const int LastNameMaxLength = 35;
        private const int EmailMaxLength = 255;
        private const int PhoneMaxLength = 35;
        private const int DepartmentMaxLength = 35;

        internal static MaxLengthValidator ValidateContact(this MaxLengthValidator validator, UpdateSolutionContactViewModel contact, string contactTag)
            => validator
                    .Validate(contact?.FirstName, FirstNameMaxLength, $"{contactTag}-first-name")
                    .Validate(contact?.LastName, LastNameMaxLength, $"{contactTag}-last-name")
                    .Validate(contact?.Email, EmailMaxLength, $"{contactTag}-email-address")
                    .Validate(contact?.PhoneNumber, PhoneMaxLength, $"{contactTag}-phone-number")
                    .Validate(contact?.Department, DepartmentMaxLength, $"{contactTag}-department-name");

    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal class UpdateSolutionContactDetailsValidator : IValidator<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult>
    {
        public ContactsMaxLengthResult Validate(UpdateSolutionContactDetailsCommand updateSolutionContactDetailsCommand)
        => new ContactsMaxLengthResult(
            new MaxLengthValidator().ValidateContact(updateSolutionContactDetailsCommand.Data.Contact1).Result(),
            new MaxLengthValidator().ValidateContact(updateSolutionContactDetailsCommand.Data.Contact2).Result());
    }

    internal static class MaxLengthValidatorContactExtensions
    {
        private const int FirstNameMaxLength = 35;
        private const int LastNameMaxLength = 35;
        private const int EmailMaxLength = 255;
        private const int PhoneMaxLength = 35;
        private const int DepartmentMaxLength = 50;

        internal static MaxLengthValidator ValidateContact(this MaxLengthValidator validator, UpdateSolutionContactViewModel contact)
            => validator
                    .Validate(contact?.FirstName, FirstNameMaxLength, "first-name")
                    .Validate(contact?.LastName, LastNameMaxLength, "last-name")
                    .Validate(contact?.Email, EmailMaxLength, "email-address")
                    .Validate(contact?.PhoneNumber, PhoneMaxLength, "phone-number")
                    .Validate(contact?.Department, DepartmentMaxLength, "department-name");

    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal static class MaxLengthValidatorContactExtensions
    {
        private const int FirstNameMaxLength = 35;
        private const int LastNameMaxLength = 35;
        private const int EmailMaxLength = 255;
        private const int PhoneMaxLength = 35;
        private const int DepartmentMaxLength = 50;

        internal static MaxLengthValidator ValidateContact(this MaxLengthValidator validator, IUpdateSolutionContact contact)
        {
            return validator
                .Validate(contact?.FirstName, FirstNameMaxLength, "first-name")
                .Validate(contact?.LastName, LastNameMaxLength, "last-name")
                .Validate(contact?.Email, EmailMaxLength, "email-address")
                .Validate(contact?.PhoneNumber, PhoneMaxLength, "phone-number")
                .Validate(contact?.Department, DepartmentMaxLength, "department-name");
        }
    }
}

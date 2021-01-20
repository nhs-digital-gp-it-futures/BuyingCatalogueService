using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    internal sealed class UpdateSolutionContactDetailsValidator : IValidator<UpdateSolutionContactDetailsCommand, ContactsMaxLengthResult>
    {
        public ContactsMaxLengthResult Validate(UpdateSolutionContactDetailsCommand command) => new(
            new MaxLengthValidator().ValidateContact(command.Data.Contact1).Result(),
            new MaxLengthValidator().ValidateContact(command.Data.Contact2).Result());
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    internal sealed class UpdateSupplierValidator : IValidator<UpdateSupplierCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSupplierCommand command)
            => new MaxLengthValidator()
                    .Validate(command.Data.Description, 1000, "description")
                    .Validate(command.Data.Link, 1000, "link")
                    .Result();
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales
{
    internal sealed class UpdateImplementationTimescalesValidator : IValidator<UpdateImplementationTimescalesCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateImplementationTimescalesCommand command)
            => new MaxLengthValidator()
                .Validate(command.Description, 1100, "description")
                .Result();
    }
}

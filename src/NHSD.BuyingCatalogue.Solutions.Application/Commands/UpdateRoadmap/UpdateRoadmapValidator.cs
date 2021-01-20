using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadMap
{
    internal sealed class UpdateRoadMapValidator : IValidator<UpdateRoadMapCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateRoadMapCommand command) => new MaxLengthValidator()
            .Validate(command.Summary, 1000, "summary")
            .Result();
    }
}

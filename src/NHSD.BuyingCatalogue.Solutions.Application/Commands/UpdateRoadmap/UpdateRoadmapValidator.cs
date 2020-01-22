using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap
{
    internal sealed class UpdateRoadmapValidator : IValidator<UpdateRoadmapCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateRoadmapCommand command)
            => new MaxLengthValidator()
                    .Validate(command.Description, 1000, "description")
                    .Result();
    }
}

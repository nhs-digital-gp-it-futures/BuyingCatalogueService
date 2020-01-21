using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud
{
    internal sealed class UpdatePublicCloudValidator : IValidator<UpdatePublicCloudCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdatePublicCloudCommand command)
            => new MaxLengthValidator()
                .Validate(command.Data.Summary, 500, "summary")
                .Validate(command.Data.Link, 1000, "link")
                .Result();
    }
}

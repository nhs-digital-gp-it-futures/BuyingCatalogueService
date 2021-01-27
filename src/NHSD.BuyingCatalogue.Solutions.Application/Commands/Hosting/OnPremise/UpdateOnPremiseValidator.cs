using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.OnPremise
{
    internal sealed class UpdateOnPremiseValidator : IValidator<UpdateOnPremiseCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateOnPremiseCommand command)
            => new MaxLengthValidator()
                .Validate(command.Data.Summary, 500, "summary")
                .Validate(command.Data.Link, 1000, "link")
                .Validate(command.Data.HostingModel, 1000, "hosting-model")
                .Result();
    }
}

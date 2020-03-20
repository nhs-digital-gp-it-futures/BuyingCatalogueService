using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionValidator : IValidator<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionConnectivityAndResolutionCommand command)
            => new RequiredValidator()
                .Validate(command.Data.MinimumConnectionSpeed, "minimum-connection-speed")
                .Result();
    }
}

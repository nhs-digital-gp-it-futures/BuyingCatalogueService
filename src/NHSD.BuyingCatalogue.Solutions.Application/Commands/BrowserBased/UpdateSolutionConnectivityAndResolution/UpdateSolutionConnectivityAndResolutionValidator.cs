using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionValidator : IValidator<UpdateSolutionConnectivityAndResolutionCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionConnectivityAndResolutionCommand command)
            => new RequiredValidator()
                .Validate(command.ViewModel.MinimumConnectionSpeed, "minimum-connection-speed")
                .Result();
    }
}

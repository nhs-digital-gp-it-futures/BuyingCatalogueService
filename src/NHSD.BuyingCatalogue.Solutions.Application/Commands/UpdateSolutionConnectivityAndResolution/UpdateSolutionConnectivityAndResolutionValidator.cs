using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionValidator : IValidator<UpdateSolutionConnectivityAndResolutionCommand, RequiredResult>
    {
        public RequiredResult Validate(UpdateSolutionConnectivityAndResolutionCommand command)
            => new RequiredValidator()
                .Validate(command.ViewModel.MinimumConnectionSpeed, "minimum-connection-speed")
                .Result();
    }
}

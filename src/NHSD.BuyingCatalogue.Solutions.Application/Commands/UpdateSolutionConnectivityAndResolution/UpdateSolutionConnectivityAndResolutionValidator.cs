using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionValidator
    {
        public RequiredResult Validation(UpdateSolutionConnectivityAndResolutionViewModel viewModel)
            => new RequiredValidator()
                .Validate(viewModel.MinimumConnectionSpeed, "minimum-connection-speed")
                .Result();
    }
}

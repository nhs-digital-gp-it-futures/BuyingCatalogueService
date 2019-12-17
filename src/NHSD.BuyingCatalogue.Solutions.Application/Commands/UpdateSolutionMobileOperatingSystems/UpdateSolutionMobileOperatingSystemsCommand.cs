using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems
{
    public class UpdateSolutionMobileOperatingSystemsCommand : IRequest<RequiredMaxLengthResult>
    {
        public string Id { get; }
        public UpdateSolutionMobileOperatingSystemsViewModel ViewModel { get; }

        public UpdateSolutionMobileOperatingSystemsCommand(string id, UpdateSolutionMobileOperatingSystemsViewModel viewModel)
        {
            Id = id;
            ViewModel = viewModel;
        }
    }
}

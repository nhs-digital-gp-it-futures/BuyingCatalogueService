using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    public class UpdateSolutionMobileOperatingSystemsCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public UpdateSolutionMobileOperatingSystemsViewModel Data { get; }

        public UpdateSolutionMobileOperatingSystemsCommand(string id, UpdateSolutionMobileOperatingSystemsViewModel data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}

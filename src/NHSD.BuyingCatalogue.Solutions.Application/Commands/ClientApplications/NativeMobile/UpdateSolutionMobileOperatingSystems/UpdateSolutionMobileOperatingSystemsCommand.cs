using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    public sealed class UpdateSolutionMobileOperatingSystemsCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public IUpdateNativeMobileOperatingSystemsData Data { get; }

        public UpdateSolutionMobileOperatingSystemsCommand(string id, IUpdateNativeMobileOperatingSystemsData data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

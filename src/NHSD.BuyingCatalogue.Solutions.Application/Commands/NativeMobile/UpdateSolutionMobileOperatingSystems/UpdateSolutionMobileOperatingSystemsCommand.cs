using System.Collections.Generic;
using System.Linq;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    public class UpdateSolutionMobileOperatingSystemsCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public UpdateSolutionMobileOperatingSystemsViewModel Data { get; }

        public UpdateSolutionMobileOperatingSystemsCommand(string id, UpdateSolutionMobileOperatingSystemsViewModel data)
        {
            Id = id.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.OperatingSystemsDescription = Data.OperatingSystemsDescription?.Trim();
            Data.OperatingSystems = Data.OperatingSystems == null ?
                new HashSet<string>() : 
                new HashSet<string>(Data.OperatingSystems.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));
        }
    }
}
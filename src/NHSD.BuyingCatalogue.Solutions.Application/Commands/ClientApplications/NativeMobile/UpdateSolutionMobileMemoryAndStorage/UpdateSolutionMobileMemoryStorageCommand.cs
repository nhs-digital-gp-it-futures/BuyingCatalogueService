using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage
{
    public class UpdateSolutionMobileMemoryStorageCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public string MinimumMemoryRequirement { get; set; }
        public string Description { get; set; }

        public UpdateSolutionMobileMemoryStorageCommand(string id, string minimumMemoryRequirement, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            MinimumMemoryRequirement = minimumMemoryRequirement;
            Description = description;
        }
    }
}

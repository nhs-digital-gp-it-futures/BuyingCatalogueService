﻿using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage
{
    internal sealed class UpdateSolutionMobileMemoryStorageValidator : IValidator<UpdateSolutionMobileMemoryStorageCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionMobileMemoryStorageCommand command) => new RequiredMaxLengthResult(
            new RequiredValidator()
                .Validate(command.MinimumMemoryRequirement, "minimum-memory-requirement")
                .Validate(command.Description, "storage-requirements-description")
                .Result(),
            new MaxLengthValidator().Validate(command.Description, 300, "storage-requirements-description")
                .Result());
    }
}

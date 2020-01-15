using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage
{
    class UpdateNativeDesktopMemoryAndStorageValidator : IValidator<UpdateNativeDesktopMemoryAndStorageCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateNativeDesktopMemoryAndStorageCommand command)
        => new RequiredMaxLengthResult(
            new RequiredValidator()
                .Validate(command.Data.MinimumMemoryRequirement, "minimum-memory-requirement")
                .Validate(command.Data.StorageRequirementsDescription, "storage-requirements-description")
                .Validate(command.Data.MinimumCpu, "minimum-cpu")
                .Result(),
            new MaxLengthValidator()
                .Validate(command.Data.StorageRequirementsDescription, 300, "storage-requirements-description")
                .Validate(command.Data.MinimumCpu, 300, "minimum-cpu")
                .Result()
            );
    }
}

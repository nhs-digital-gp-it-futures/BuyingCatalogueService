using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.PrivateCloud
{
    internal sealed class UpdatePrivateCloudHandler : Handler<UpdatePrivateCloudCommand, ISimpleResult>
    {
        public UpdatePrivateCloudHandler(
            IExecutor<UpdatePrivateCloudCommand> executor,
            IValidator<UpdatePrivateCloudCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}

using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud
{
    internal sealed class UpdatePublicCloudHandler : Handler<UpdatePublicCloudCommand, ISimpleResult>
    {
        public UpdatePublicCloudHandler(IExecutor<UpdatePublicCloudCommand> executor,
            IValidator<UpdatePublicCloudCommand, ISimpleResult> validator) : base(executor, validator)
        {
        }
    }
}

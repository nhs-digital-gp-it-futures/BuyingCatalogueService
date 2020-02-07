using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class CapabilityValidator : IVerifier<UpdateCapabilitiesCommand, ISimpleResult>
    {
        private readonly VerifyCapabilityResult _verifyCapabilityResult;

        private readonly CapabilityVerifier _verifier;

        internal CapabilityValidator(CapabilityVerifier verifier)
        {
            _verifier = verifier;
            _verifyCapabilityResult = new VerifyCapabilityResult();
        }

        public async Task<ISimpleResult> Verify(UpdateCapabilitiesCommand command)
        {
            var result = await _verifier.CheckCapabilityReferenceExists(command.NewCapabilitiesReferences, new CancellationToken()).ConfigureAwait(false);
            if (!result)
            {
                _verifyCapabilityResult.CapabilityValid.Add("Capability Invalid");
            }
            return _verifyCapabilityResult;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class CapabilityVerifier : IVerifier<UpdateCapabilitiesCommand, ISimpleResult>
    {
        private readonly ISolutionCapabilityRepository _solutionCapabilityRepository;
        private readonly VerifyCapabilityResult _verifyCapabilityResult;

        public CapabilityVerifier(ISolutionCapabilityRepository solutionCapabilityRepository)
        {
            _solutionCapabilityRepository = solutionCapabilityRepository;
            _verifyCapabilityResult = new VerifyCapabilityResult();
        }

        public async Task<ISimpleResult> Verify(UpdateCapabilitiesCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var result = await CheckCapabilityReferenceExists(command.NewCapabilitiesReferences, new CancellationToken()).ConfigureAwait(false);
            if (!result)
            {
                _verifyCapabilityResult.CapabilityValid.Add("capabilities");
            }
            return _verifyCapabilityResult;
        }

        public async Task<bool> CheckCapabilityReferenceExists(IEnumerable<string> newCapabilitiesReferences,
            CancellationToken cancellationToken)
        {
            var count = await _solutionCapabilityRepository.CheckCapabilitiesFromReferenceExistAsync(newCapabilitiesReferences,
                cancellationToken).ConfigureAwait(false);

            return count == newCapabilitiesReferences.ToList().Count;
        }
    }
}

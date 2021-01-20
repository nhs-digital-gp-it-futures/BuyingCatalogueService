using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class CapabilityVerifier : IVerifier<UpdateCapabilitiesCommand, ISimpleResult>
    {
        private readonly ISolutionCapabilityRepository solutionCapabilityRepository;
        private readonly VerifyCapabilityResult verifyCapabilityResult;

        public CapabilityVerifier(ISolutionCapabilityRepository solutionCapabilityRepository)
        {
            this.solutionCapabilityRepository = solutionCapabilityRepository;
            verifyCapabilityResult = new VerifyCapabilityResult();
        }

        public async Task<ISimpleResult> VerifyAsync(UpdateCapabilitiesCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var result = await CheckCapabilityReferenceExists(command.NewCapabilitiesReferences, CancellationToken.None);
            if (!result)
            {
                verifyCapabilityResult.ValidCapabilityList.Add("capabilities");
            }

            return verifyCapabilityResult;
        }

        public async Task<bool> CheckCapabilityReferenceExists(
            IEnumerable<string> capabilitiesToMatch,
            CancellationToken cancellationToken)
        {
            var count = await solutionCapabilityRepository.GetMatchingCapabilitiesCountAsync(
                capabilitiesToMatch,
                cancellationToken);

            return count == capabilitiesToMatch.ToList().Count;
        }
    }
}

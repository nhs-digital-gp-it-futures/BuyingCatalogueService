using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;
using NHSD.BuyingCatalogue.Domain.Entities.Organisations;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionListReader
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository _solutionRepository;

        public SolutionListReader(ISolutionRepository solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task<IEnumerable<Solution>> ListAsync(ISet<Guid> capabilityIdList, CancellationToken cancellationToken)
        {
            if (capabilityIdList is null)
            {
                throw new System.ArgumentNullException(nameof(capabilityIdList));
            }

            var allSolutions = await _solutionRepository.ListAsync(cancellationToken).ConfigureAwait(false);

            var solutionList = Map(allSolutions).Distinct();

            if (capabilityIdList.Any())
            {
                solutionList = solutionList.Where(solution => capabilityIdList.Intersect(solution.Capabilities.Select(capability => capability.Id)).Count() == capabilityIdList.Count());
            }

            return solutionList;
        }

        private IEnumerable<Solution> Map(IEnumerable<ISolutionListResult> solutionListResults)
        {
            Dictionary<string, Solution> solutionDictionary = new Dictionary<string, Solution>();

            foreach (var item in solutionListResults)
            {
                if (!solutionDictionary.TryGetValue(item.SolutionId, out Solution currentSolution))
                {
                    var solution = new Solution
                    {
                        Id = item.SolutionId,
                        Name = item.SolutionName,
                        Summary = item.SolutionSummary,
                        Organisation = new Organisation
                        {
                            Id = item.OrganisationId,
                            Name = item.OrganisationName
                        }
                    };
                    solutionDictionary.Add(item.SolutionId, solution);

                    currentSolution = solution;
                }

                currentSolution.AddCapability(new Capability { Id = item.CapabilityId, Name = item.CapabilityName, Description = item.CapabilityDescription });

            }

            return solutionDictionary.Values;
        }
    }
}


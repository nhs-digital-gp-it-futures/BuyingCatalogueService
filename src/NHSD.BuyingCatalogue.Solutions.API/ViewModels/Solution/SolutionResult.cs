using System;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class SolutionResult
    {
        public string Id { get; }

        public string Name { get; }

        public string OrganisationName { get; }

        public bool? IsFoundation { get; }

        public DateTime? LastUpdated { get; }

        public Sections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionResult"/> class.
        /// </summary>
        public SolutionResult(ISolution solution)
        {
            if (solution != null)
            {
                Id = solution.Id;
                Name = solution.Name;
                OrganisationName = solution.OrganisationName;
                LastUpdated = solution.LastUpdated;
                IsFoundation = solution.IsFoundation;

                Sections = new Sections(solution);
            }
        }
    }
}

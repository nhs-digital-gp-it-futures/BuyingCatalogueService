using System;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class SolutionResult
    {
        public string Id { get; }

        public string Name { get; }

        public string OrganisationName { get; }

        public bool IsFoundation { get; }

        public string LastUpdated { get; }

        public Sections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionResult"/> class.
        /// </summary>
        public SolutionResult(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Id = solution.Id;
            Name = solution.Name;
            OrganisationName = solution.OrganisationName;
            LastUpdated = solution.LastUpdated.ToString("dd-MMM-yyyy");
            IsFoundation = solution.IsFoundation;

            Sections = new Sections(solution);
        }
    }
}

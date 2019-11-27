using System;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class SolutionPreviewResult
    {
        public string Id { get; }

        public string Name { get; }

        public string OrganisationName { get; }

        public PreviewSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionPreviewResult"/> class.
        /// </summary>
        public SolutionPreviewResult(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Id = solution.Id;
            Name = solution.Name;
            OrganisationName = solution.OrganisationName;
            Sections = new PreviewSections(solution);
        }
    }
}

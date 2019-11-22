using NHSD.BuyingCatalogue.API.ViewModels.Public;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solution.API.ViewModels.Public
{
    public class SolutionDescriptionPublicSection
    {
        public SolutionDescriptionPublicSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionPublicSection"/> class.
        /// </summary>
        public SolutionDescriptionPublicSection(ISolution solution)
        {
            Answers = new SolutionDescriptionPublicSectionAnswers(solution);
        }

        public SolutionDescriptionPublicSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

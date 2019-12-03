using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class SolutionDescriptionSection
    {
        public SolutionDescriptionSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionSection"/> class.
        /// </summary>
        public SolutionDescriptionSection(ISolution solution)
        {
            Answers = new SolutionDescriptionSectionAnswers(solution);
        }

        public SolutionDescriptionSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

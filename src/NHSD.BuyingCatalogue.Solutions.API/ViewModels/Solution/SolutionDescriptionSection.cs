using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class SolutionDescriptionSection
    {
        public SolutionDescriptionSection(ISolution solution)
        {
            Answers = new SolutionDescriptionSectionAnswers(solution);
        }

        public SolutionDescriptionSectionAnswers Answers { get; }

        public SolutionDescriptionSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

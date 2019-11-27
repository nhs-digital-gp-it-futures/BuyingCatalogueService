using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class SolutionDescriptionPreviewSection
    {
        public SolutionDescriptionPreviewSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionPreviewSection"/> class.
        /// </summary>
        public SolutionDescriptionPreviewSection(ISolution solution)
        {
            Answers = new SolutionDescriptionPreviewSectionAnswers(solution);
        }

        public SolutionDescriptionPreviewSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

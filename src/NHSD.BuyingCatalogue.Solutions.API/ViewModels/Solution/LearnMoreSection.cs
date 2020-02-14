using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class LearnMoreSection
    {
        public LearnMoreSection(ILearnMore learnMore) => Answers = new LearnMoreSectionAnswers(learnMore);

        public LearnMoreSectionAnswers Answers { get; }

        internal LearnMoreSection IfPopulated() => Answers.HasData ? this : null;
    }
}

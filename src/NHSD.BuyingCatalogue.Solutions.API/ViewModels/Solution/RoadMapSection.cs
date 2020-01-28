namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class RoadMapSection
    {
        public RoadMapSectionAnswers Answers { get; }

        public RoadMapSection(string summary)
        {
            Answers = new RoadMapSectionAnswers(summary);
        }

        public RoadMapSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

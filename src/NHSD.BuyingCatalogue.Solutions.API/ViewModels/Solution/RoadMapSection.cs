using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class RoadMapSection
    {
        public RoadMapSection(IRoadMap roadMap)
        {
            Answers = new RoadMapSectionAnswers(roadMap);
        }

        public RoadMapSectionAnswers Answers { get; }

        public RoadMapSection IfPopulated() => Answers.HasData ? this : null;
    }
}

using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class RoadMapSection
    {
        public RoadMapSectionAnswers Answers { get; }

        public RoadMapSection(IRoadMap roadMap)
        {
            Answers = new RoadMapSectionAnswers(roadMap);
        }

        public RoadMapSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

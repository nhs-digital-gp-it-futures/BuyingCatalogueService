using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class PrivateCloudSection
    {
        internal PrivateCloudSection(IHosting hosting)
        {
            Answers = new PrivateCloudSectionAnswers(hosting?.PrivateCloud);
        }

        public PrivateCloudSectionAnswers Answers { get; }

        public PrivateCloudSection IfPopulated() => Answers.HasData ? this : null;
    }
}

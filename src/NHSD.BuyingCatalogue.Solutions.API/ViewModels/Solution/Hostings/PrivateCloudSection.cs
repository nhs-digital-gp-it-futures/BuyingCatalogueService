using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class PrivateCloudSection
    {
        public PrivateCloudSectionAnswers Answers { get; }

        internal PrivateCloudSection(IHosting hosting)
        {
            Answers = new PrivateCloudSectionAnswers(hosting?.PrivateCloud);
        }

        public PrivateCloudSection IfPopulated()
            => Answers.HasData ? this : null;
    }
}

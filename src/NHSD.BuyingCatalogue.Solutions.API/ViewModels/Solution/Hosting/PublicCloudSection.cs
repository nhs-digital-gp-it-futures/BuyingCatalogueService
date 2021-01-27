using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hosting
{
    public sealed class PublicCloudSection
    {
        internal PublicCloudSection(IHosting hosting)
        {
            Answers = new PublicCloudSectionAnswers(hosting?.PublicCloud);
        }

        public PublicCloudSectionAnswers Answers { get; }

        public PublicCloudSection IfPopulated()
            => Answers.HasData ? this : null;
    }
}

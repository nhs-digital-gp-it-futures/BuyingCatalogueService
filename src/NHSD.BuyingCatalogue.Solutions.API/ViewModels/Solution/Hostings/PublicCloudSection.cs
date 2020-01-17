using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class PublicCloudSection
    {
        public PublicCloudSectionAnswers Answers { get; }

        internal PublicCloudSection(IHosting hosting)
        {
            Answers = new PublicCloudSectionAnswers(hosting?.PublicCloud);
        }

        public PublicCloudSection IfPopulated()
            => Answers.HasData ? this : null;
    }
}

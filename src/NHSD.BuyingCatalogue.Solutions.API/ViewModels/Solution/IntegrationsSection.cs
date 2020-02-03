namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class IntegrationsSection
    {
        public IntegrationsSectionAnswers Answers { get; }

        public IntegrationsSection(string integrationsUrl)
        {
            Answers = new IntegrationsSectionAnswers(integrationsUrl);
        }

        public IntegrationsSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

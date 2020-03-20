using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class IntegrationsSection
    {
        public IntegrationsSectionAnswers Answers { get; }

        public IntegrationsSection(IIntegrations integration)
        {
            Answers = new IntegrationsSectionAnswers(integration);
        }

        public IntegrationsSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}

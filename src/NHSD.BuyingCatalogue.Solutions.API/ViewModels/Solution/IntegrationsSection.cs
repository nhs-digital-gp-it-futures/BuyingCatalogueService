using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class IntegrationsSection
    {
        public IntegrationsSection(IIntegrations integration)
        {
            Answers = new IntegrationsSectionAnswers(integration);
        }

        public IntegrationsSectionAnswers Answers { get; }

        public IntegrationsSection IfPopulated() => Answers.HasData ? this : null;
    }
}

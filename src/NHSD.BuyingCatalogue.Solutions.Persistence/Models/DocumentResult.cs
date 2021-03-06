using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class DocumentResult : IDocumentResult
    {
        public string RoadMapDocumentName { get; set; }

        public string IntegrationDocumentName { get; set; }

        public string SolutionDocumentName { get; set;  }
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;
using IDocumentsAPIClient = NHSD.BuyingCatalogue.Solutions.Persistence.Clients.IDocumentsAPIClient;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the Solution entity.
    /// </summary>
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly IDocumentsAPIClient _client;

        public DocumentRepository(IDocumentsAPIClient client, ISettings settings)
        {
            _client = client.ThrowIfNull();
            _client.BaseAddress = new Uri(settings.ThrowIfNull().DocumentApiBaseUrl);
        }

        public async Task<IDocumentResult> GetDocumentResultBySolutionIdAsync(string solutionId,
            CancellationToken cancellationToken)
        {
            var documents = await _client.DocumentsAsync(solutionId, cancellationToken).ConfigureAwait(false);
            return new DocumentResult
            {
                RoadMapDocumentName = documents.OrderByDescending(x => x)
                    .FirstOrDefault(x => x.Contains("RoadMap", StringComparison.InvariantCultureIgnoreCase))
            };
        }
    }
}

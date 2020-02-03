using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Clients;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;
using IDocumentsAPIClient = NHSD.BuyingCatalogue.Solutions.Persistence.Clients.IDocumentsAPIClient;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the Solution entity.
    /// </summary>
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly ILogger<DocumentRepository> _logger;
        private readonly IDocumentsAPIClient _client;

        public DocumentRepository(IDocumentsAPIClient client, ISettings settings, ILogger<DocumentRepository> logger)
        {
            _logger = logger;
            _client = client.ThrowIfNull();
            _client.BaseAddress = new Uri(settings.ThrowIfNull().DocumentApiBaseUrl);
        }

        public async Task<IDocumentResult> GetDocumentResultBySolutionIdAsync(string solutionId,
            CancellationToken cancellationToken)
        {
            try
            {
                var documents = await _client.DocumentsAsync(solutionId, cancellationToken).ConfigureAwait(false);
                return new DocumentResult
                {
                    RoadMapDocumentName = documents.OrderByDescending(x => x)
                        .FirstOrDefault(x => x.Contains("RoadMap", StringComparison.InvariantCultureIgnoreCase))
                };
            }
            catch (ApiException e)
            {
                _logger.LogError(e,"Call to {baseAddress} failed", _client.BaseAddress);
            }
            return new DocumentResult();
        }
    }
}

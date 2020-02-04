using System;
using System.Linq;
using System.Net.Http;
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
    /// Represents the data access layer for the Documents Api.
    /// </summary>
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly ISettings _settings;
        private readonly ILogger<DocumentRepository> _logger;
        private readonly IDocumentsAPIClient _client;

        public DocumentRepository(IDocumentsAPIClient client, ISettings settings, ILogger<DocumentRepository> logger)
        {
            _settings = settings.ThrowIfNull();
            _logger = logger.ThrowIfNull();
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
                        .FirstOrDefault(x => x.Contains(_settings.DocumentRoadMapIdentifier,
                            StringComparison.InvariantCultureIgnoreCase)),
                    IntegrationDocumentName = documents.OrderByDescending(x => x)
                        .FirstOrDefault(x => x.Contains(_settings.DocumentIntegrationIdentifier,
                            StringComparison.InvariantCultureIgnoreCase))
                };
            }
            catch (ApiException e)
            {
                _logger.LogError(e, "Call to {baseAddress} failed with Api Error", _client.BaseAddress);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Call to {baseAddress} failed with Http Request Error", _client.BaseAddress);
            }
            return new DocumentResult();
        }
    }
}

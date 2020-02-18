using System;
using System.Collections.Generic;
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
    internal sealed class DocumentRepository : IDocumentRepository
    {
        private readonly IDocumentsAPIClient _client;
        private readonly ILogger<DocumentRepository> _logger;
        private readonly ISettings _settings;

        public DocumentRepository(IDocumentsAPIClient client, ISettings settings, ILogger<DocumentRepository> logger)
        {
            _settings = settings.ThrowIfNull();
            _logger = logger.ThrowIfNull();
            _client = client.ThrowIfNull();
            _client.BaseAddress = new Uri(settings.ThrowIfNull().DocumentApiBaseUrl);
        }

        public async Task<IDocumentResult> GetDocumentResultBySolutionIdAsync(
            string solutionId,
            CancellationToken cancellationToken)
        {
            try
            {
                var documents = await _client.DocumentsAsync(solutionId, cancellationToken).ConfigureAwait(false);
                var sortedDocuments = documents.OrderByDescending(d => d).ToList();

                return new DocumentResult
                {
                    RoadMapDocumentName = FindDocument(sortedDocuments, _settings.DocumentRoadMapIdentifier),
                    IntegrationDocumentName = FindDocument(sortedDocuments, _settings.DocumentIntegrationIdentifier),
                    SolutionDocumentName = FindDocument(sortedDocuments, _settings.DocumentSolutionIdentifier),
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

        private static string FindDocument(IEnumerable<string> sortedDocuments, string identifier) =>
            sortedDocuments.FirstOrDefault(d => d.Contains(identifier, StringComparison.OrdinalIgnoreCase));
    }
}

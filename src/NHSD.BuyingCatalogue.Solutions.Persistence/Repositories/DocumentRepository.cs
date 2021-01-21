using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
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
        private readonly IDocumentsAPIClient client;
        private readonly ILogger<DocumentRepository> logger;
        private readonly ISettings settings;

        public DocumentRepository(IDocumentsAPIClient client, ISettings settings, ILogger<DocumentRepository> logger)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.client.BaseAddress = new Uri(settings.DocumentApiBaseUrl);
        }

        public async Task<IDocumentResult> GetDocumentResultBySolutionIdAsync(
            string solutionId,
            CancellationToken cancellationToken)
        {
            try
            {
                var documents = await client.DocumentsAsync(solutionId, cancellationToken).ConfigureAwait(false);
                var sortedDocuments = documents.OrderByDescending(d => d).ToList();

                return new DocumentResult
                {
                    RoadMapDocumentName = FindDocument(sortedDocuments, settings.DocumentRoadMapIdentifier),
                    IntegrationDocumentName = FindDocument(sortedDocuments, settings.DocumentIntegrationIdentifier),
                    SolutionDocumentName = FindDocument(sortedDocuments, settings.DocumentSolutionIdentifier),
                };
            }
            catch (ApiException e)
            {
                logger.LogError(e, "Call to {baseAddress} failed with Api Error", client.BaseAddress);
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "Call to {baseAddress} failed with Http Request Error", client.BaseAddress);
            }

            return new DocumentResult();
        }

        private static string FindDocument(IEnumerable<string> sortedDocuments, string identifier) =>
            sortedDocuments.FirstOrDefault(d => d.Contains(identifier, StringComparison.OrdinalIgnoreCase));
    }
}

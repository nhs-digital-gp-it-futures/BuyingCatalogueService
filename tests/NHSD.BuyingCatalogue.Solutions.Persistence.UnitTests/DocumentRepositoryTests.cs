using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Persistence.Clients;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.UnitTests
{
    [TestFixture]
    internal sealed class DocumentRepositoryTests
    {
        private const string SampleUrl = "http://localhost/";
        private const string RoadMapIdentifier = "RoadMap";
        private const string DocumentIntegrationIdentifier = "Integration";
        private const string DocumentSolutionIdentifier = "Solution";
        private const string SolutionId = "Sln1";

        private Mock<IDocumentsAPIClient> apiClientMock;
        private CancellationToken cancellationToken;
        private Mock<ILogger<DocumentRepository>> loggerMock;
        private Mock<ISettings> settingsMock;

        [SetUp]
        public void Setup()
        {
            apiClientMock = new Mock<IDocumentsAPIClient>();
            apiClientMock.SetupAllProperties();

            settingsMock = new Mock<ISettings>();
            settingsMock.SetupGet(s => s.DocumentApiBaseUrl).Returns(SampleUrl);
            settingsMock.SetupGet(s => s.DocumentRoadMapIdentifier).Returns(RoadMapIdentifier);
            settingsMock.SetupGet(s => s.DocumentIntegrationIdentifier).Returns(DocumentIntegrationIdentifier);
            settingsMock.SetupGet(s => s.DocumentSolutionIdentifier).Returns(DocumentSolutionIdentifier);

            loggerMock = new Mock<ILogger<DocumentRepository>>();
            cancellationToken = CancellationToken.None;
        }

        [TestCase(new[] { "RoadMap.pdf" }, "RoadMap.pdf", null, null)]
        [TestCase(new[] { "NotMapped" }, null, null, null)]
        [TestCase(new[] { "Integration" }, null, "Integration", null)]
        [TestCase(new[] { "Solution.pdf" }, null, null, "Solution.pdf")]
        [TestCase(new[] { "RoadMap", "Integration.pdf", "Solution.pdf" }, "RoadMap", "Integration.pdf", "Solution.pdf")]
        public async Task ShouldGetDocumentsBySolutionId(
            string[] clientResult,
            string roadMapDocumentName,
            string integrationDocumentName,
            string solutionDocumentName)
        {
            apiClientMock
                .Setup(api => api.DocumentsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResult);

            var sut = new DocumentRepository(apiClientMock.Object, settingsMock.Object, loggerMock.Object);

            var result = await sut.GetDocumentResultBySolutionIdAsync(SolutionId, cancellationToken);

            result.RoadMapDocumentName.Should().Be(roadMapDocumentName);
            result.IntegrationDocumentName.Should().Be(integrationDocumentName);
            result.SolutionDocumentName.Should().Be(solutionDocumentName);

            apiClientMock.Verify(api => api.DocumentsAsync(SolutionId, CancellationToken.None));
        }

        [Test]
        public void ShouldSetBaseUrl()
        {
            var unused = new DocumentRepository(apiClientMock.Object, settingsMock.Object, loggerMock.Object);
            apiClientMock.Object.BaseAddress.AbsoluteUri.Should().Be(SampleUrl);
        }

        [Test]
        public async Task ShouldLogErrorReturnEmptyResultOnApiException()
        {
            apiClientMock
                .Setup(api => api.DocumentsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiException("Api Failure", 500, "Response", null, null));

            var sut = new DocumentRepository(apiClientMock.Object, settingsMock.Object, loggerMock.Object);

            var result = await sut.GetDocumentResultBySolutionIdAsync(SolutionId, cancellationToken);

            result.RoadMapDocumentName.Should().BeNullOrEmpty();

            Expression<Action<ILogger<DocumentRepository>>> expression = l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true));

            loggerMock.Verify(expression);
        }

        [Test]
        public async Task ShouldLogErrorReturnEmptyResultOnHttpRequestException()
        {
            apiClientMock
                .Setup(api => api.DocumentsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException("message"));

            var sut = new DocumentRepository(apiClientMock.Object, settingsMock.Object, loggerMock.Object);

            var result = await sut.GetDocumentResultBySolutionIdAsync(SolutionId, cancellationToken);

            result.RoadMapDocumentName.Should().BeNullOrEmpty();

            Expression<Action<ILogger<DocumentRepository>>> expression = l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true));

            loggerMock.Verify(expression);
        }
    }
}

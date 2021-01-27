using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.HostingTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hosting
{
    [TestFixture]
    internal sealed class GetHostingBySolutionIdTests
    {
        private TestContext context;
        private string solutionId;
        private CancellationToken cancellationToken;
        private IHostingResult hostingResult;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
            solutionId = "Sln1";
            cancellationToken = CancellationToken.None;
            context.MockSolutionDetailRepository
                .Setup(r => r.GetHostingBySolutionIdAsync(solutionId, cancellationToken))
                .ReturnsAsync(() => hostingResult);
        }

        [Test]
        public async Task ShouldGetHostingById()
        {
            var originalHosting = new Application.Domain.Hosting
            {
                PublicCloud = new PublicCloud
                {
                    Summary = "Some Summary",
                    Link = "www.somelink.com",
                    RequiresHSCN = "This Solution requires a HSCN/N3 connection",
                },
                PrivateCloud = new PrivateCloud
                {
                    Summary = "Private Summary",
                    Link = "www.privatelink.com",
                    HostingModel = "Hosting Model",
                    RequiresHSCN = "How much wood would a woodchuck chuck if a woodchuck could chuck wood?",
                },
                HybridHostingType = new HybridHostingType
                {
                    Summary = "Private Summary",
                    Link = "www.privatelink.com",
                    HostingModel = "Hosting Model",
                    RequiresHSCN = "This Solution requires a HSCN/N3 connection",
                },
                OnPremise = new OnPremise
                {
                    Summary = "Private Summary",
                    Link = "www.privatelink.com",
                    HostingModel = "Hosting Model",
                    RequiresHSCN = "This Solution requires a HSCN/N3 connection",
                },
            };

            Expression<Func<IHostingResult, bool>> hostingResultExpression = r =>
                r.Id == solutionId
                && r.Hosting == JsonConvert.SerializeObject(originalHosting);

            hostingResult = Mock.Of(hostingResultExpression);

            var newHosting = await context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(solutionId), cancellationToken);

            newHosting.Should().BeEquivalentTo(originalHosting);
        }

        [Test]
        public async Task EmptyHostingResultReturnsDefaultHosting()
        {
            hostingResult = Mock.Of<IHostingResult>(r => r.Id == solutionId && r.Hosting == null);

            var hosting = await context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(solutionId), cancellationToken);

            hosting.Should().NotBeNull();
            hosting.Should().BeEquivalentTo(new HostingDto());
        }

        [Test]
        public void NullHostingResultThrowsNotFoundException()
        {
            hostingResult = null;

            Assert.ThrowsAsync<NotFoundException>(() => context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(solutionId),
                cancellationToken));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class PricingControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private PricingController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new PricingController(_mockMediator.Object);
        }

        //[Test]
        //public async Task ShouldGetPricing()
        //{
        //    _mockMediator.Setup(m => m.Send(It.Is<GetPricingBySolutionIdQuery>(q => q.SolutionId == "Sln1"),
        //        It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IPricingUnit>())
        //}

    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Capabilities.Queries
{
    [TestFixture]
    public sealed class ListCapabilitiesHandlerTests
    {
        private Mock<ICapabilityRepository> _repository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<ICapabilityRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Handle_CallsRepository_Once()
        {
            //ARRANGE
            var testData = CapabilityListTestData.Two();

            _repository.Setup(x => x.ListAsync(CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new ListCapabilitiesHandler(new CapabilityReader(_repository.Object), _mapper.Object);

            //ACT
            var _ = await testObject.Handle(new ListCapabilitiesQuery(), CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ListAsync(CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var testData = CapabilityListTestData.Two();

            _repository.Setup(x => x.ListAsync(CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new ListCapabilitiesHandler(new CapabilityReader(_repository.Object), _mapper.Object);

            //ACT
            _ = await testObject.Handle(new ListCapabilitiesQuery(), CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map<IEnumerable<CapabilityViewModel>>(It.IsAny<IEnumerable<Capability>>()), Times.Once);
        }

        [Test]
        public async Task Handle_NoData_ReturnsEmpty()
        {
            //ARRANGE
            var testObject = new ListCapabilitiesHandler(new CapabilityReader(_repository.Object), _mapper.Object);

            //ACT
            var result = await testObject.Handle(new ListCapabilitiesQuery(), CancellationToken.None);

            //ASSERT
            result.ShouldNotBeNull();
            result.Capabilities.ShouldBeEmpty();
        }

        [Test]
        public async Task Handle_Data_ReturnsData()
        {
            //ARRANGE
            var testData = CapabilityListTestData.Two();
            var mapRes = new List<CapabilityViewModel>();

            _repository.Setup(x => x.ListAsync(CancellationToken.None)).Returns(() => Task.FromResult(testData));
            _mapper.Setup(x => x.Map<IEnumerable<CapabilityViewModel>>(It.Is<IEnumerable<Capability>>(src => src == testData)))
              .Returns(mapRes);

            var testObject = new ListCapabilitiesHandler(new CapabilityReader(_repository.Object), _mapper.Object);

            //ACT
            var result = await testObject.Handle(new ListCapabilitiesQuery(), CancellationToken.None);

            //ASSERT
            result.Capabilities.ShouldBe(mapRes);
        }
    }
}
